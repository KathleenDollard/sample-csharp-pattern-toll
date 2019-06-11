using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace ExternalSystem
{

    public static class BillingSystem
    {
        public static IResult<object> SendBill(this IResult<(decimal toll, object vehicle)> tollResult)
        {
            if (tollResult is SuccessResult<(decimal toll, object vehicle)> successResult)
            {
                var toll = successResult.Value.toll;
                var vehicle = successResult.Value.vehicle;
                if (toll < 0)
                {
                    return Result.Fail<object>("very oops");
                }

                switch (vehicle)
                {
                    case null:
                        return Result.Fail<object>("oops");
                    case CarRegistration car:
                        return SendCustomerBill(toll, car);
                    case TaxiRegistration taxi:
                        return SendCustomerBill(toll, taxi);
                    case BusRegistration bus:
                        return SendCustomerBill(toll, bus);
                    case DeliveryTruckRegistration truck:
                        return SendCustomerBill(toll, truck);
                }
            }
            return Result.Fail<object>("oops");
        }

        // The following methods are stubbed until the links to these systems is created
        private static IResult<object> SendCustomerBill(decimal toll, DeliveryTruckRegistration truck)
        {
            Logger.LogInfo($"Truck ({truck.LicensePlate}) - {toll}");
            return Result.Success<object>(Guid.NewGuid());
        }

        private static IResult<object> SendCustomerBill(decimal toll, BusRegistration bus)
        {
            Logger.LogInfo($"Bus ({bus.LicensePlate}) - {toll}");
            return Result.Success<object>(Guid.NewGuid());
        }

        private static IResult<object> SendCustomerBill(decimal toll, TaxiRegistration taxi)
        {
            Logger.LogInfo($"Taxi ({taxi.LicensePlate}) - {toll}");
            return Result.Success<object>(Guid.NewGuid());
        }

        private static IResult<object> SendCustomerBill(decimal toll, CarRegistration car)
        {
            Logger.LogInfo($"Car ({car.LicensePlate}) - {toll}");
            return Result.Success<object>(Guid.NewGuid());
        }
    }
}
