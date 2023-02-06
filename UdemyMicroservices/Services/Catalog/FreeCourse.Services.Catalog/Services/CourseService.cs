using AutoMapper;
using FreeCource.Shared.Dtos;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDataBaseSettings dataBaseSettings)
        {
            var client = new MongoClient(dataBaseSettings.ConnectionString);
            var database = client.GetDatabase(dataBaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(dataBaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(dataBaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
             var courses=await _courseCollection.Find(course=>true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category=await _categoryCollection.Find<Category>(x=>x.Id==course.CategoryId).FirstAsync();

                }
            }
            else
            {
                courses=new List<Course> { };
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CategoryCreateDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
             

        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
             
        }





    }
}
