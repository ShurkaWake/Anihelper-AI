namespace API.Responses;

public record ResponseModel<T>
    (T Data,
    string[] Errors);

public record ResponseModel
    (string[] Errors);

public record PagingResponseModel<T>
    (IEnumerable<T> Data,
    string[] Errors,
    int PageCount);