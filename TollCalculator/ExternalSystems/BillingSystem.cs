using CommercialRegistration;
using Common;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace TollRunner.ExternalSystems
{


    public class BillingSystem
    {
        public Result<Guid> SendBill(object vehicle)
        {
            if (vehicle == null)
            {
                return new Result<Guid>(ResultStatus.Error, Guid.Empty );
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
            return new Result<Guid>(ResultStatus.Failure, Guid.NewGuid());
        }

        // The following methods are stubbed until the links to these systems is created
        private Result<Guid> SendCustomerBill(DeliveryTruckRegistration truck)
            => new Result<Guid>(ResultStatus.Success, Guid.NewGuid());

        private Result<Guid> SendCustomerBill(BusRegistration bus)
            => new Result<Guid>(ResultStatus.Success, Guid.NewGuid());

        private Result<Guid> SendCustomerBill(TaxiRegistration taxi)
            => new Result<Guid>(ResultStatus.Success, Guid.NewGuid());

        private Result<Guid> SendCustomerBill(CarRegistration car) 
            => new Result<Guid>(ResultStatus.Success, Guid.NewGuid());
    }
}
