using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Appointments;
using Clinic_BusinessLayer.DTOs.Responses.Appointments;
using Clinic_BusinessLayer.Exceptions;
using Clinic_BusinessLayer.Helpers;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuditLogService _auditLog;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IAuditLogService auditLog)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _auditLog = auditLog;
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAllAsync()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentResponse>>(appointments);
        }

        public async Task<AppointmentResponse> GetByIdAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Appointment with id {id} not found");

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<IEnumerable<AppointmentResponse>> GetByPatientAsync(int patientId)
        {
            var appointments = await _unitOfWork.Appointments.GetByPatientAsync(patientId);
            return _mapper.Map<IEnumerable<AppointmentResponse>>(appointments);
        }

        public async Task<IEnumerable<AppointmentResponse>> GetByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var appointments = await _unitOfWork.Appointments.GetByDoctorAndDateAsync(doctorId, date);
            return _mapper.Map<IEnumerable<AppointmentResponse>>(appointments);
        }

        public async Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request, int createdByUserId)
        {
            var patient = await _unitOfWork.Patients.GetWithPersonAsync(request.PatientId);
            if (patient == null || patient.DeletedAt != null)
                throw new NotFoundException($"Patient with id {request.PatientId} not found");

            var doctor = await _unitOfWork.Doctors.GetWithPersonAsync(request.DoctorId);
            if (doctor == null || doctor.DeletedAt != null)
                throw new NotFoundException($"Doctor with id {request.DoctorId} not found");

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId);
            if (branch == null || branch.DeletedAt != null)
                throw new NotFoundException($"Branch with id {request.BranchId} not found");

            if (!SlotHelper.IsValidSlot(request.StartDate))
                throw new BadRequestException("Appointment must start on a 15-minute slot");

            var isBooked = await _unitOfWork.Appointments.IsSlotBookedAsync(
                request.DoctorId, request.StartDate);

            if (isBooked)
                throw new ConflictException("This appointment slot is already booked");

            var appointment = _mapper.Map<Appointment>(request);
            appointment.CreatedBy = createdByUserId;
            appointment.EndDate = SlotHelper.CalculateEndTime(request.StartDate);
            appointment.Status = "Scheduled";

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            await _auditLog.LogAsync(createdByUserId, "Appointments", appointment.Id, "Created", null, "Appointment Created");

            appointment.Patient = patient;
            appointment.Doctor = doctor;
            appointment.Branch = branch;

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<AppointmentResponse> UpdateAsync(int id, UpdateAppointmentRequest request)
        {
            var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Appointment with id {id} not found");

            if (appointment.StartDate != request.StartDate)
            {
                if (!SlotHelper.IsValidSlot(request.StartDate))
                    throw new BadRequestException("Appointment must start on a 15-minute slot");

                var isBooked = await _unitOfWork.Appointments.IsSlotBookedAsync(
                    appointment.DoctorId, request.StartDate, id);

                if (isBooked)
                    throw new ConflictException("This appointment slot is already booked");

                appointment.EndDate = SlotHelper.CalculateEndTime(request.StartDate);
            }

            _mapper.Map(request, appointment);
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<AppointmentResponse> CancelAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Appointment with id {id} not found");

            if (appointment.Status == "Cancelled")
                throw new ConflictException("Appointment is already cancelled");

            if (appointment.StartDate - DateTime.UtcNow < TimeSpan.FromHours(2))
                throw new BadRequestException("Cannot cancel appointment less than 2 hours before it starts");

            appointment.Status = "Cancelled";
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
            if (appointment == null)
                throw new NotFoundException($"Appointment with id {id} not found");

            _unitOfWork.Appointments.Remove(appointment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
