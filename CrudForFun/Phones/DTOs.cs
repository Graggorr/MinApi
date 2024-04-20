using WebStore.Domain;

namespace WebStore.API.Phones;

public record CreatePhoneRequest(string phone);
public record CreatePhoneResponse(Phone phone); 

public record UpdatePhoneRequest(int Id);
public record UpdatePhoneResponse(Phone phone);

public record DeletePhoneRequest(int Id);

public record GetPhoneRequest(int Id);

public record GetPhoneResponse(Phone phone);
