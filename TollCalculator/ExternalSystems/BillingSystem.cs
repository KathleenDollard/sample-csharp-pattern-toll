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
            if (toll < 0)
            {
                return Result<object>.Failure("Tolls can't be less than zero");
            }

            switch (vehicle)
            {
                case null:
                    return Result<object>.Failure("Vehicle registration can't be null when sending a bill");
                case CarRegistration car:
                    return SendCustomerBill(toll, car);
                case TaxiRegistration taxi:
                    return SendCustomerBill(toll, taxi);
                case BusRegistration bus:
                    return SendCustomerBill(toll, bus);
                case DeliveryTruckRegistration truck:
                    return SendCustomerBill(toll, truck);
            }
            return Result<object>.Failure("Unexpected Registration type");
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
