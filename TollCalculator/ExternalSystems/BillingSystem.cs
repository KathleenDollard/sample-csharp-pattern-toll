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
                return SendCustomerBill(car);
            }

            if (vehicle is TaxiRegistration taxi)
            {
                return SendCustomerBill(taxi);
            }

            if (vehicle is BusRegistration bus)
            {
                return SendCustomerBill(bus);
            }

            if (vehicle is DeliveryTruckRegistration truck)
            {
                return SendCustomerBill(truck);
            }
            return Result<object>.Failure("oops");
        }

        // The following methods are stubbed until the links to these systems is created
        private Result<object> SendCustomerBill(DeliveryTruckRegistration truck)
            => Result<object>.Success(Guid.NewGuid());

        private Result<object> SendCustomerBill(BusRegistration bus)
            => Result<object>.Success(Guid.NewGuid());

        private Result<object> SendCustomerBill(TaxiRegistration taxi)
            => Result<object>.Success(Guid.NewGuid());

        private Result<object> SendCustomerBill(CarRegistration car) 
            => Result<object>.Success(Guid.NewGuid());
    }
}
