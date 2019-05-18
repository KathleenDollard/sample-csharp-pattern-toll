using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace ExternalSystem
{


    public class BillingSystem
    {
        public Result<Guid> SendBill(decimal toll, object vehicle)
        {
            if (vehicle == null)
            {
                return Result<Guid>.Failure("oops");
            }

            if (toll < 0)
            {
                return Result<Guid>.Failure("very oops");
            }

            var car = vehicle as CarRegistration;
            if (car != null)
            {
                return SendCustomerBill(car);
            }

            var taxi = vehicle as TaxiRegistration;
            if (taxi != null)
            {
                return SendCustomerBill(taxi);
            }

            var bus = vehicle as BusRegistration;
            if (bus != null)
            {
                return SendCustomerBill(bus);
            }

            var truck = vehicle as DeliveryTruckRegistration;
            if (truck != null)
            {
                return SendCustomerBill(truck);
            }
            return Result<Guid>.Failure("oops");
        }

        // The following methods are stubbed until the links to these systems is created
        private Result<Guid> SendCustomerBill(DeliveryTruckRegistration truck)
            => Result<Guid>.Success(Guid.NewGuid());

        private Result<Guid> SendCustomerBill(BusRegistration bus)
            => Result<Guid>.Success(Guid.NewGuid());

        private Result<Guid> SendCustomerBill(TaxiRegistration taxi)
            => Result<Guid>.Success(Guid.NewGuid());

        private Result<Guid> SendCustomerBill(CarRegistration car) 
            => Result<Guid>.Success(Guid.NewGuid());
    }
}
