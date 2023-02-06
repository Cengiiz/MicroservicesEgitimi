using AutoMapper;
using FreeCource.Shared.Dtos;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CourseService: ICourseService
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
                    course.Category=await _categoryCollection.Find(x=>x.Id==course.CategoryId).FirstAsync();

                }
            }
            else
            {
                courses=new List<Course> { };
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courceCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courceCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
             var course=await _courseCollection.Find<Course>(x=>x.Id== id).FirstOrDefaultAsync();
            if (course==null)
            {
                return Response<CourseDto>.Fail("Course Not Fount", 404);
            }
            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x =>x.UserId==userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

                }
            }
            else
            {
                courses = new List<Course> { };
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courceUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courceUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x=>x.Id==courceUpdateDto.Id, updateCourse);
            if (result==null)
            {
                return Response<NoContent>.Fail("Course not fount",404);
            }
            return Response<NoContent>.Success(204);

        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result=await _courseCollection.DeleteOneAsync(x=>x.Id==id);
            if (result.DeletedCount>0)
            {
                return Response<NoContent>.Success(204);
            }
            else
                return Response<NoContent>.Fail("Course not fount",404);
        }

    }
}
