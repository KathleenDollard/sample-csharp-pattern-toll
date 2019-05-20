using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace ExternalSystem
{


    public class BillingSystem
    {
        public IResult<object> SendBill(decimal toll, object vehicle)
        {
            if (vehicle == null)
            {
                return Result<object>.Failure("oops");
            }

            if (toll < 0)
            {
                return Result<object>.Failure("very oops");
            }

            if (vehicle is CarRegistration car)
            {
                return SendCustomerBill(toll,car);
            }

            if (vehicle is TaxiRegistration taxi)
            {
                return SendCustomerBill(toll, taxi);
            }

            if (vehicle is BusRegistration bus)
            {
                return SendCustomerBill(toll, bus);
            }

            if (vehicle is DeliveryTruckRegistration truck)
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
