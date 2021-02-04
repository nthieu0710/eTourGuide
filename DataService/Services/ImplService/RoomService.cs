using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public List<RoomResponse> GetRoomForExhibit(int[] exhibitId)
        {
            //Check ở nhánh Event
            //Get List ExhibitInEvent vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInEvent> listExhibitInEvent = new List<eTourGuide.Data.Entity.ExhibitInEvent>();
            foreach (int item in exhibitId)
            {
                var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>().GetAll().Where(x => x.ExhibitId == item).FirstOrDefault();
                if (exhibitInEvent != null)
                {
                    listExhibitInEvent.Add((ExhibitInEvent)exhibitInEvent);
                }
            }
            //Kiểm tra Event nào có Status == 1
            List<eTourGuide.Data.Entity.ExhibitInEvent> listExhibitInEventCheckCondition = new List<ExhibitInEvent>();
            if (listExhibitInEvent != null)
            {
                foreach (var item in listExhibitInEvent)
                {
                    if (item.Event.Status == 1)
                    {
                        listExhibitInEventCheckCondition.Add(item);
                    }
                }
            }
            //Get Room 
            List<RoomResponse> listRoom = new List<RoomResponse>();
            if (listExhibitInEventCheckCondition != null)
            {
                foreach (var item in listExhibitInEventCheckCondition)
                {
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
                    RoomResponse roomRs = new RoomResponse() 
                    {
                        Id = room.Id,
                        Floor = (int)room.Floor,
                        No = (int)room.No,
                        Status = (int)room.Status
                    };                  
                    if (listRoom.Count == 0)
                    {
                        listRoom.Add(roomRs);
                    }
                    foreach (var r in listRoom)
                    {
                        if (r.Id != roomRs.Id)
                        {
                            listRoom.Add(roomRs);
                        }
                    }
                }
            }


            //Check ở nhánh Event
            //Get List ExhibitInEvent vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInTopic> listExhibitInTopic = new List<eTourGuide.Data.Entity.ExhibitInTopic>();
            foreach (int item in exhibitId)
            {
                var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.ExhibitId == item).FirstOrDefault();
                if (exhibitInTopic != null)
                {
                    listExhibitInTopic.Add(exhibitInTopic);
                }
            }
            //Kiểm tra Topic nào có Status == 1
            List<eTourGuide.Data.Entity.ExhibitInTopic> listExhibitInTopicCheckCondition = new List<eTourGuide.Data.Entity.ExhibitInTopic>();
            if (listExhibitInTopic != null)
            {
                foreach (var item in listExhibitInTopic)
                {
                    if (item.Topic.Status == 1)
                    {
                        listExhibitInTopicCheckCondition.Add(item);
                    }
                }
            }
            //Get Room 
            if (listExhibitInTopicCheckCondition != null)
            {
                foreach (var item in listExhibitInTopicCheckCondition)
                {
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
                    RoomResponse roomRs = new RoomResponse()
                    {
                        Id = room.Id,
                        Floor = (int)room.Floor,
                        No = (int)room.No,
                        Status = (int)room.Status
                    };                   
                    if (listRoom.Count == 0)
                    {
                        listRoom.Add(roomRs);
                    }

                    for (int i = 0; i < listRoom.Count; i++)
                    {
                        if (listRoom[i].Id != roomRs.Id)
                        {
                            listRoom.Add(roomRs);
                        }
                    }
                }

            }
            return listRoom;          
        }
    }
}
