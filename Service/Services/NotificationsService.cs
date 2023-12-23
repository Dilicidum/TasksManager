﻿using AutoMapper;
using Domain.Interfaces;
using Services.Abstractions.DTO;
using Services.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationsService(IUnitOfWork unitOfWork,IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetNotifications(string userId)
        {
            var tasks = (await _unitOfWork.TasksRepository.GetDueTasksForToday(userId));
            
            var notifications = _mapper.Map<Notification[]>(tasks);

            return notifications;
        }


    }
}
