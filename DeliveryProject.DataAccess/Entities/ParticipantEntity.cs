﻿namespace DeliveryProject.DataAccess.Entities
{
    public abstract class ParticipantEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Rating { get; set; }
    }
}