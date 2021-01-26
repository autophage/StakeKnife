using NodaMoney;
using System;
using System.Collections.Generic;
using System.Linq;
using static StakeKnife.BackEnd.Entities;

namespace StakeKnife.DomainActions
{
    public class PaymentCalculator
    {
        public List<Payment> CalculatePaymentsForProject(Project project, Money totalToDisburse)
        {
            var payments = new List<Payment>();

            var payees = project
                .Buckets
                .SelectMany(s => s.Shares)
                .Select(s => s.Stakeholder)
                .Distinct();

            foreach (var bucket in project.Buckets)
            {
                var singleShareValue = totalToDisburse * bucket.Value;

                foreach (var share in bucket.Shares)
                {
                    var amount = singleShareValue * share.Value;
                    var payment = new Payment(project, amount, share.Stakeholder, PaymentStatus.Created);
                    payments.Add(payment);
                }
            }

            return Consolidate(payments);
        }

        public List<Payment> Consolidate(List<Payment> payments)
        {
            var consolidatedPayments = new List<Payment>();

            foreach(var payment in payments)
            {
                var existingPayment = consolidatedPayments.SingleOrDefault(p => p.Stakeholder == payment.Stakeholder);

                if (existingPayment != null)
                {
                    existingPayment.Amount += payment.Amount;
                }
                else
                {
                    consolidatedPayments.Add(payment);
                }
            }

            return consolidatedPayments;
        }
    }
}
