using AutoMapper;
using HotelBookingSystem.Enums;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace HotelBookingSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly iUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ReservationController(iUnitOfWork _unitOfWork , IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet] 
        public IActionResult create()
        {
           List<hotelBranch> branches = unitOfWork.hotelRepository.GetAll();
            ViewBag.Branches = branches;
            return View();
        }


        public IActionResult checkReservationAvailability(generalReservationViewModel model)
        {
           availableRoomsViewModel availableSingleRooms = 
                checkAvailableRooms(new roomReservationViewModel
            {
                checkIn = model.checkIn,
                checkOut = model.checkOut,
                roomsNum = model.singleRoomsNum,
                branchId = model.branchId
            }, RoomType.Single);

            availableRoomsViewModel availableDoubleRooms =
               checkAvailableRooms(new roomReservationViewModel
               {
                   checkIn = model.checkIn,
                   checkOut = model.checkOut,
                   roomsNum = model.doubleRoomsNum,
                   branchId = model.branchId
               }, RoomType.Double);

            availableRoomsViewModel availableSuitRooms =
              checkAvailableRooms(new roomReservationViewModel
              {
                  checkIn = model.checkIn,
                  checkOut = model.checkOut,
                  roomsNum = model.suitRoomsNum,
                  branchId = model.branchId
              }, RoomType.Suite);


            return Json(new {availableSingleRooms , availableDoubleRooms , availableSuitRooms });
        }


        public availableRoomsViewModel checkAvailableRooms(roomReservationViewModel model , RoomType type)
        {
            int availableRooms = 0;
            bool isReservationOverlapped = false;
            List<Room> rooms = unitOfWork.roomRepository
                .Get(r => r.hotelId == model.branchId && r.type == type, "reservations");

            foreach (Room room in rooms)
            {
                if (room.reservations.Count == 0)
                {
                    availableRooms++;
                }
                else
                {
                    foreach (Reservation reservation in room.reservations)
                    {
                        if (reservation.checkIn >= model.checkOut ||
                            reservation.checkout <= model.checkIn)
                        {
                            isReservationOverlapped = false;
                        }
                        else
                        {
                            isReservationOverlapped = true;
                            break;
                        }
                    }
                    if (!isReservationOverlapped)
                    {
                        availableRooms++;
                    }
                }
            }
            if (availableRooms >= model.roomsNum)
            {
                return new availableRoomsViewModel
                {
                    isAvailable = true,
                    availableNum = availableRooms
                };
            }
            return new availableRoomsViewModel
            {
                isAvailable = false,
                availableNum = availableRooms
            };
        }


        public async Task <IActionResult> calculateTotalCost(generalReservationViewModel model)
        {
            decimal singleRoomsCost = model.singleRoomsNum * (int)RoomType.Single * (model.checkOut - model.checkIn).Days; 
            decimal doubleRoomsCost = model.doubleRoomsNum * (int)RoomType.Double * (model.checkOut - model.checkIn).Days; 
            decimal suitRoomsCost = model.suitRoomsNum * (int)RoomType.Suite * (model.checkOut - model.checkIn).Days;
            decimal totalCost = singleRoomsCost + doubleRoomsCost + suitRoomsCost;
            ApplicationUser user = await unitOfWork.userManager.FindByNameAsync(User.Identity.Name);
            if(user.isOldClient)
            {
                totalCost -= (totalCost * .05m);
            }
            return Json(new { totalCost , isOldClient = user.isOldClient });
        }


        
        public List<Room> getAvailableRooms(roomReservationViewModel model , RoomType type)
        {
            List<Room> rooms = new List<Room>();
          rooms = unitOfWork.roomRepository
                .Get(r => r.hotelId == model.branchId && r.type == type , "reservations");

            List<Room> filteredRooms = new List<Room>();

            bool isReservationOverlapped = false;

            foreach (Room room in rooms) 
            {
                if(filteredRooms.Count == model.roomsNum)
                {
                    break;
                }

                if (room.reservations.Count == 0)
                {
                    filteredRooms.Add(room);
                }

                else
                {
                    foreach (Reservation reservation in room.reservations)
                    {
                        if (reservation.checkIn >= model.checkOut ||
                            reservation.checkout <= model.checkIn)
                        {
                            isReservationOverlapped = false;
                        }
                        else
                        {
                            isReservationOverlapped = true;
                            break;
                        }
                    }

                    if (!isReservationOverlapped)
                    {
                        filteredRooms.Add(room);
                    }
                }
            }
            return filteredRooms;

        }

        public async Task<IActionResult> book(generalReservationViewModel model , string jsonData)
        {
            int counter = 0;
            List<int>? roomsCapacity = JsonConvert.DeserializeObject<List<int>>(jsonData);



            roomReservationViewModel singleRoomsReservationViewModel = new roomReservationViewModel() { 
            branchId = model.branchId,
            checkIn = model.checkIn,
            checkOut = model.checkOut,
            roomsNum = model.singleRoomsNum
            };
            List<Room> singleRooms = getAvailableRooms(singleRoomsReservationViewModel, RoomType.Single);

            int RoomsCapacityIndex = 0;
            foreach (Room room in singleRooms ) 
            {
                unitOfWork.reservationRepository.Insert(new Reservation
                {
                    clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    roomId = room.id,
                    checkIn = model.checkIn,
                    nationalId = model.nationalId,
                    phoneNumber = model.phoneNumber,
                    branchId = model.branchId,
                    checkout = model.checkOut,
                    name = model.name,
                    cost = (int)singleRooms[RoomsCapacityIndex].type * ((model.checkOut - model.checkIn).Days),
                    adultsNum = roomsCapacity[RoomsCapacityIndex * 2],
                    childrenNum = roomsCapacity[RoomsCapacityIndex * 2 + 1]
                });
                RoomsCapacityIndex++;
            }

            roomReservationViewModel doubleRoomsReservationViewModel = new roomReservationViewModel()
            {
                branchId = model.branchId,
                checkIn = model.checkIn,
                checkOut = model.checkOut,
                roomsNum = model.doubleRoomsNum
            };
            List<Room> doubleRooms = getAvailableRooms(doubleRoomsReservationViewModel, RoomType.Double);

            foreach (Room room in doubleRooms)
            {
                unitOfWork.reservationRepository.Insert(new Reservation
                {
                    clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    roomId = room.id,
                    checkIn = model.checkIn,
                    nationalId = model.nationalId,
                    phoneNumber = model.phoneNumber,
                    branchId = model.branchId,
                    checkout = model.checkOut,
                    name = model.name,
                    cost = (int) room.type * ((model.checkOut - model.checkIn).Days),
                    adultsNum = roomsCapacity[RoomsCapacityIndex * 2],
                    childrenNum = roomsCapacity[RoomsCapacityIndex * 2 + 1]
                });
                RoomsCapacityIndex++;
            }

            roomReservationViewModel suiteRoomsReservationViewModel = new roomReservationViewModel()
            {
                branchId = model.branchId,
                checkIn = model.checkIn,
                checkOut = model.checkOut,
                roomsNum = model.suitRoomsNum
            };
            List<Room> suitRooms = getAvailableRooms(suiteRoomsReservationViewModel, RoomType.Suite);

            foreach(Room room in suitRooms) 
            {
                unitOfWork.reservationRepository.Insert(new Reservation
                {
                    clientId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    roomId = room.id,
                    checkIn = model.checkIn,
                    nationalId = model.nationalId,
                    phoneNumber = model.phoneNumber,
                    branchId = model.branchId,
                    checkout = model.checkOut,
                    name = model.name,
                    cost = (int)room.type * ((model.checkOut - model.checkIn).Days),
                    adultsNum = roomsCapacity[RoomsCapacityIndex * 2],
                    childrenNum = roomsCapacity[RoomsCapacityIndex * 2 + 1]
                });
                RoomsCapacityIndex++;
            }

            unitOfWork.reservationRepository.Save();
            ApplicationUser user = await unitOfWork.userManager.FindByNameAsync(User.Identity.Name);
            user.isOldClient = true;
            Task<IdentityResult> result = unitOfWork.userManager.UpdateAsync(user);

            ViewBag.singleRooms = singleRooms;
            ViewBag.doubleRooms = doubleRooms;
            ViewBag.suitRooms = suitRooms;

            await result;
            return View("successfullbooking");  
        }

     //   [Authorize("Admin")]
        public IActionResult getAll()
        {
          List<Reservation> reservations = unitOfWork.reservationRepository.GetAll();

          List<reservationsViewModel> reservationViewModels =
                mapper.Map<List<reservationsViewModel>>(reservations);

            for(int i = 0; i< reservations.Count; i++)
            {
              hotelBranch branch = unitOfWork.hotelRepository.GetById(reservations[i].branchId);
                reservationViewModels[i].branchName = branch.name;
            }
            return View(reservationViewModels);

        }


        [HttpGet] 
        public IActionResult Details(string clientId , int roomId , DateTime checkIn)
        {
            Reservation? reservation = unitOfWork.reservationRepository
                                     .Get(r => r.clientId == clientId && r.roomId == roomId && r.checkIn == checkIn)
                                     .FirstOrDefault();
            if(reservation != null) 
            {
                reservationsViewModel reservationViewModel =
                                   mapper.Map<reservationsViewModel>(reservation);
                hotelBranch branch = unitOfWork.hotelRepository.GetById(reservation.branchId);
                reservationViewModel.branchName = branch.name;
                return View(reservationViewModel);
            }

            return View(reservation);

        }

    }
}
