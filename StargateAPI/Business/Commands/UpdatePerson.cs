using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands
{
    public class UpdatePerson : IRequest<UpdatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
        
        public required string NewName { get; set; } = string.Empty;
    }

    public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
    {
        private readonly StargateContext _context;
        
        public UpdatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        
        public Task Process(UpdatePerson request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);
            
            if (person is null) throw new BadHttpRequestException("Person not found.");
            
            var otherPerson = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.NewName);

            if (otherPerson is not null) throw new BadHttpRequestException("Person with name already exists.");

            return Task.CompletedTask;
        }
    }

    public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
    {
        private readonly StargateContext _context;

        public UpdatePersonHandler(StargateContext context)
        {
            _context = context;
        }
        
        public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
        {
            var query = $"SELECT * FROM [Person] WHERE \'{request.Name}\' = Name";

            var person = await _context.Connection.QueryFirstOrDefaultAsync<Person>(query);
            person.Name = request.NewName;
            
            _context.Update(person);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePersonResult()
            {
                Id = person.Id
            };
        }
    }

    public class UpdatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
