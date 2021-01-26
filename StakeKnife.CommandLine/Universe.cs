using StakeKnife.BackEnd;
using System;
using System.Collections.Generic;
using System.Text;
using static StakeKnife.BackEnd.Entities;

namespace StakeKnife.CommandLine
{
    [Serializable]
    internal sealed class Universe
    {
        public List<Address> Addresses;
        public List<User> Users;
        public List<AuditEntry> AuditTrail;
        public List<Share> Shares;
        public List<Project> Projects;
        public List<Stakeholder> Stakeholders;
        public List<Studio> Studios;
        public List<Payment> Payments;
        public List<PaymentEvent> PaymentEvents;
    }
}
