using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace ExternalSystem
{


    public class BillingSystem
    {
        public Result<object> SendBill(decimal toll, object vehicle)
        {
            if (vehicle == null)
            {
                return Result<object>.Failure("oops");
            }

            if (toll < 0)
            {
                return Result<object>.Failure("very oops");
            }

            var car = vehicle as CarRegistration;
            if (car != null)
            {
                return SendCustomerBill(toll,car);
            }

            var taxi = vehicle as TaxiRegistration;
            if (taxi != null)
            {
                return SendCustomerBill(toll, taxi);
            }

            var bus = vehicle as BusRegistration;
            if (bus != null)
            {
                return SendCustomerBill(toll, bus);
            }

            var truck = vehicle as DeliveryTruckRegistration;
            if (truck != null)
            {
                return SendCustomerBill(toll, truck);
            }
            return Result<object>.Failure("oops");
        }

        // The following methods are stubbed until the links to these systems is created
        private Result<object> SendCustomerBill(decimal toll, DeliveryTruckRegistration truck)
        {
            Logger.LogInfo($"Truck ({truck.LicensePlate}) - {toll}");
            return Result<object>.Success(Guid.NewGuid());
        }

        private Result<object> SendCustomerBill(decimal toll, BusRegistration bus)
        {
            Logger.LogInfo($"Bus ({bus.LicensePlate}) - {toll}");
            return Result<object>.Success(Guid.NewGuid());
        }

        private Result<object> SendCustomerBill(decimal toll, TaxiRegistration taxi)
        {
            Logger.LogInfo($"Taxi ({taxi.LicensePlate}) - {toll}");
            return Result<object>.Success(Guid.NewGuid());
        }

        private Result<object> SendCustomerBill(decimal toll, CarRegistration car)
        {
            Logger.LogInfo($"Car ({car.LicensePlate}) - {toll}");
            return Result<object>.Success(Guid.NewGuid());
        }
    }
}
