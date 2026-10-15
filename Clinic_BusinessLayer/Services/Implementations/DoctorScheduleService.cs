using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.DoctorSchedules;
using Clinic_BusinessLayer.DTOs.Responses.DoctorSchedules;
using Clinic_BusinessLayer.Exceptions;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorScheduleResponse>> GetAllAsync()
        {
            var schedules = await _unitOfWork.DoctorSchedules.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorScheduleResponse>>(schedules);
        }

        public async Task<DoctorScheduleResponse> GetByIdAsync(int id)
        {
            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(id);
            if (schedule == null)
                throw new NotFoundException($"Schedule with id {id} not found");

            return _mapper.Map<DoctorScheduleResponse>(schedule);
        }

        public async Task<IEnumerable<DoctorScheduleResponse>> GetByDoctorAsync(int doctorId)
        {
            var schedules = await _unitOfWork.DoctorSchedules.GetByDoctorAsync(doctorId);
            return _mapper.Map<IEnumerable<DoctorScheduleResponse>>(schedules);
        }

        public async Task<DoctorScheduleResponse> CreateAsync(CreateDoctorScheduleRequest request)
        {
            var doctor = await _unitOfWork.Doctors.GetWithPersonAsync(request.DoctorId);
            if (doctor == null || doctor.DeletedAt != null)
                throw new NotFoundException($"Doctor with id {request.DoctorId} not found");

            var overlapping = await _unitOfWork.DoctorSchedules.IsOverlappingAsync(
                request.DoctorId, request.DayOfWeek, request.StartTime, request.EndTime);

            if (overlapping)
                throw new ConflictException("Schedule overlaps with existing schedule");

            var schedule = _mapper.Map<DoctorSchedule>(request);
            await _unitOfWork.DoctorSchedules.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            schedule.Doctor = doctor;

            return _mapper.Map<DoctorScheduleResponse>(schedule);
        }

        public async Task<DoctorScheduleResponse> UpdateAsync(int id, UpdateDoctorScheduleRequest request)
        {
            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(id);
            if (schedule == null)
                throw new NotFoundException($"Schedule with id {id} not found");

            var overlapping = await _unitOfWork.DoctorSchedules.IsOverlappingAsync(
                schedule.DoctorId, request.DayOfWeek, request.StartTime, request.EndTime, id);

            if (overlapping)
                throw new ConflictException("Schedule overlaps with existing schedule");

            _mapper.Map(request, schedule);
            _unitOfWork.DoctorSchedules.Update(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorScheduleResponse>(schedule);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(id);
            if (schedule == null)
                throw new NotFoundException($"Schedule with id {id} not found");

            _unitOfWork.DoctorSchedules.Remove(schedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}