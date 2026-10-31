using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Business_Logic_Layer.DTO.RegistrationPdfDto;
using Business_Logic_Layer.Service.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Business_Logic_Layer.Service.Implementation
{
    public class RegistrationPdfService : IRegistrationPdfService
    {
        private readonly HttpClient _httpClient;

        public RegistrationPdfService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> GenerateRegistrationPdfAsync(RegistrationPdfDto dto)
        {


            var navy = "#1A2233";
            var blue = "#3B82F6";
            var purple = "#7C3AED";
            var lightBg = "#F4F6FA";

            var statusColor = GetStatusColor(dto.RegistrationStatus);

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.DefaultTextStyle(x =>
                        x.FontSize(11).FontColor(navy));

                    // HEADER
                    page.Header()
                        .Background(navy)
                        .Padding(18)
                        .AlignCenter()
                        .Text("EVENT REGISTRATION")
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.White);

                    // CONTENT
                    page.Content().PaddingVertical(18).Column(col =>
                    {
                        col.Spacing(16);



                        // EVENT INFORMATION
                        col.Item()
                            .Background(lightBg)
                            .Border(1)
                            .BorderColor(blue)
                            .CornerRadius(10)
                            .Padding(18)
                            .Column(eventCol =>
                            {
                                eventCol.Spacing(12);

                                eventCol.Item()
                                    .Text(dto.EventTitle ?? "Event")
                                    .FontSize(22)
                                    .Bold()
                                    .FontColor(blue);

                                eventCol.Item().Row(row =>
                                {
                                    row.Spacing(15);

                                    row.RelativeItem().Column(date =>
                                    {
                                        date.Item().Text("EVENT DATE")
                                            .FontSize(9).FontColor(purple);

                                        date.Item().Text(dto.EventDate.ToString("dd MMM yyyy"))
                                            .Bold();
                                    });

                                    row.RelativeItem().Column(time =>
                                    {
                                        time.Item().Text("TIME")
                                            .FontSize(9).FontColor(purple);

                                        time.Item().Text(dto.EventDate.ToString("hh:mm tt"))
                                            .Bold();
                                    });
                                    row.RelativeItem().Column(description =>
                                    {
                                        description.Item().Text("DESCRIPTION")
                                            .FontSize(9).FontColor(purple);

                                        description.Item().Text(dto.Description ?? "N/A")
                                            .Bold();
                                    });

                                    row.RelativeItem().Column(location =>
                                    {
                                        location.Item().Text("LOCATION")
                                            .FontSize(9).FontColor(purple);

                                        location.Item().Text(dto.EventLocation ?? "N/A")
                                            .Bold();
                                    });
                                });



                            });

                        // TICKET PRICE
                        col.Item()
                            .Background("#EFF6FF")
                            .CornerRadius(8)
                            .Padding(14)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Text("TICKET PRICE")
                                    .FontSize(12)
                                    .Bold();

                                row.RelativeItem()
                                    .AlignRight()
                                    .Text($"{dto.TicketPrice:N2} {dto.TicketCurrency ?? ""}")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(blue);
                            });

                        // REGISTRATION ID
                        col.Item()
                            .Background(blue)
                            .CornerRadius(8)
                            .Padding(12)
                            .AlignCenter()
                            .Text($"REGISTRATION ID: {dto.RegistrationId}")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.White);

                        // ATTENDEE INFORMATION
                        col.Item()
                            .Border(1)
                            .BorderColor("#E5E7EB")
                            .CornerRadius(8)
                            .Padding(14)
                            .Column(attendee =>
                            {
                                attendee.Spacing(8);

                                attendee.Item()
                                    .Text("ATTENDEE INFORMATION")
                                    .FontSize(13)
                                    .Bold()
                                    .FontColor(purple);

                                attendee.Item().Text(text =>
                                {
                                    text.Span("Name: ").Bold();
                                    text.Span(dto.AttendeeName ?? "N/A");
                                });

                                attendee.Item().Text(text =>
                                {
                                    text.Span("Email: ").Bold();
                                    text.Span(dto.AttendeeEmail ?? "N/A");
                                });
                            });

                        // REGISTRATION INFORMATION
                        col.Item()
                            .Border(1)
                            .BorderColor("#E5E7EB")
                            .CornerRadius(8)
                            .Padding(14)
                            .Column(registration =>
                            {
                                registration.Spacing(8);

                                registration.Item()
                                    .Text("REGISTRATION INFORMATION")
                                    .FontSize(13)
                                    .Bold()
                                    .FontColor(purple);

                                registration.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("Status: ").Bold();
                                        text.Span(dto.RegistrationStatus ?? "Unknown")
                                            .FontColor(statusColor);
                                    });

                                    row.RelativeItem().AlignRight().Text(text =>
                                    {
                                        text.Span("Created By: ").Bold();
                                        text.Span(dto.RegistrationCreatedBy ?? "N/A");
                                    });
                                });

                                registration.Item().Text(text =>
                                {
                                    text.Span("Registration Date: ").Bold();
                                    text.Span(
                                        dto.RegistrationDate?.ToString("dd MMM yyyy - hh:mm tt")
                                        ?? "N/A");
                                });
                            });
                    });

                    // FOOTER
                    page.Footer().Column(footer =>
                    {
                        footer.Spacing(6);

                        footer.Item()
                            .LineHorizontal(0.5f)
                            .LineColor("#E5E7EB");

                        footer.Item()
                            .AlignCenter()
                            .Text("Thank you for registering! We look forward to seeing you.")
                            .FontSize(10)
                            .Italic()
                            .FontColor("#6B7280");

                        footer.Item().AlignRight().Text(text =>
                        {
                            text.Span("Page ").FontSize(9);
                            text.CurrentPageNumber().FontSize(9);
                            text.Span(" of ").FontSize(9);
                            text.TotalPages().FontSize(9);
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }



        private static string GetStatusColor(string? status)
        {
            return status?.ToLowerInvariant() switch
            {
                "active" => "#22C55E",
                "scheduled" => "#3B82F6",
                "completed" => "#6366F1",
                "canceled" => "#EF4444",
                _ => "#6B7280"
            };
        }
    }
}
