using AutoMapper;
using Clinic_BusinessLayer.DTOs.Requests.Visits;
using Clinic_BusinessLayer.DTOs.Responses.Visits;
using Clinic_BusinessLayer.Services.Interfaces;
using Clinic_DataAccess.Entities;
using Clinic_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Services.Implementations
{
    public class VisitService : IVisitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VisitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VisitResponse>> GetAllAsync()
        {
            var visits = await _unitOfWork.Visits.GetAllAsync();
            return _mapper.Map<IEnumerable<VisitResponse>>(visits);
        }

        public async Task<VisitResponse?> GetByIdAsync(int id)
        {
            var visit = await _unitOfWork.Visits.GetWithDetailsAsync(id);
            if (visit == null)
                return null;

            return _mapper.Map<VisitResponse>(visit);
        }

        public async Task<VisitResponse?> GetByAppointmentAsync(int appointmentId)
        {
            var visit = await _unitOfWork.Visits.GetByAppointmentAsync(appointmentId);
            if (visit == null)
                return null;

            return _mapper.Map<VisitResponse>(visit);
        }

        public async Task<VisitResponse> CreateAsync(CreateVisitRequest request)
        {
            // 1. التحقق من الـ Appointment
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {request.AppointmentId} not found");

            // 2. التحقق إن مش فيه Visit تانية
            var existing = await _unitOfWork.Visits.GetByAppointmentAsync(request.AppointmentId);
            if (existing != null)
                throw new InvalidOperationException("This appointment already has a visit");

            // 3. إنشاء الـ Visit
            var visit = _mapper.Map<Visit>(request);
            await _unitOfWork.Visits.AddAsync(visit);

            // 4. تحديث حالة الـ Appointment
            appointment.Status = "Completed";
            _unitOfWork.Appointments.Update(appointment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<VisitResponse>(visit);
        }

        public async Task<VisitResponse> UpdateAsync(int id, UpdateVisitRequest request)
        {
            var visit = await _unitOfWork.Visits.GetByIdAsync(id);
            if (visit == null)
                throw new KeyNotFoundException($"Visit with id {id} not found");

            _mapper.Map(request, visit);
            _unitOfWork.Visits.Update(visit);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<VisitResponse>(visit);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var visit = await _unitOfWork.Visits.GetByIdAsync(id);
            if (visit == null)
                return false;

            _unitOfWork.Visits.Remove(visit);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
