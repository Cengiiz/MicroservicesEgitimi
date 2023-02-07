﻿using FreeCourse.Services.Catalog.Model;

namespace FreeCourse.Services.Catalog.Dtos
{
    internal class CourseCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string UserId { get; set; }

        public FeatureDto Feature { get; set; }

        public string CategoryId { get; set; }

    }
}