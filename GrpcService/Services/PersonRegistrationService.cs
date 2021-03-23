using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PersonService.Protos;
using PersonService.DataAccess;
using Microsoft.Extensions.Logging;

namespace PersonRegistrationService.Services
{

    public class PersonRegistrationService : PersonRegistration.PersonRegistrationBase
    {
        private readonly PersonOperations personOperations = new PersonOperations();
        private readonly ILogger<PersonRegistrationService> _logger;
        public PersonRegistrationService(ILogger<PersonRegistrationService> logger)
        {
            _logger = logger;
        }
        public override Task<AddPersonResponse> AddPerson(AddPersonRequest request, ServerCallContext context)
        {
            var person = request.Person;
            var people = personOperations.AddPerson(person);

            _logger.Log(LogLevel.Information, "Added person: " + person.Name);
            return Task.FromResult(new AddPersonResponse() { Status = AddPersonResponse.Types.Status.Success });
        }

        public override Task GetAllPersons(Empty request, IServerStreamWriter<GetAllPersonsResponse> responseStream, ServerCallContext context)
        {

            _logger.Log(LogLevel.Information, "GetAllPersons called!");
            var persons = personOperations.GetPeople();

            var response = new GetAllPersonsResponse();
            response.Person.AddRange(persons);
            return Task.FromResult(response);

        }

    }
}
