using Event.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Infrastructure.Seeders;

public static class EventSeeder
{
    public static void SeedEvents(this EntityTypeBuilder<Domain.Event> builder)
    {
        builder.HasData([
            new Domain.Event
            {
                Id = Guid.Parse("009bb19a-04eb-4b43-8578-85e95743c9f9"),
                Name = "Birthday Liza",
                Description = "It is my birthday.",
                CreatorId = "3e11656a-0d45-4937-80b2-4769e29539ac",
                DateOfEvent = DateTime.Parse("08.09.2003"),
                Recurrence = Enum.Parse<RecurrenceType>("Yearly"),
                
            },
            new Domain.Event
            {
                Id = Guid.Parse("ed88cd27-a01e-4373-a21a-8e37d4550b72"),
                Name = "Birthday Arya",
                Description = "It is a birthday of my dog.",
                CreatorId = "3e11656a-0d45-4937-80b2-4769e29539ac",
                DateOfEvent = DateTime.Parse("12.05.2015"),
                Recurrence = Enum.Parse<RecurrenceType>("Yearly"),
            },
            new Domain.Event
            {
                Id = Guid.Parse("8d4e7893-a795-4ff7-96d0-7cf2baa7c6e4"),
                Name = "Relationship",
                CreatorId = "f40af0d8-f360-42c2-ae5a-5859e137cda0",
                DateOfEvent = DateTime.Parse("28.07.2023"),
                Recurrence = Enum.Parse<RecurrenceType>("Monthly"),
            },
            new Domain.Event
            {
                Id = Guid.Parse("e308dad1-4c32-48c6-b7c6-1aeb7294e24a"),
                Name = "Prom",
                Description = "It is a celebration prom party.",
                CreatorId = "3e11656a-0d45-4937-80b2-4769e29539ac",
                DateOfEvent = DateTime.Parse("04.07.2025"),
            },
        ]);
    }
}