using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result(bool IsSuccess, string? errorMessage = null, ResultKind resultKind = ResultKind.Ok)
    {
        public static Result Ok() => new(true);
        public static Result Fail(string erorrMessage, ResultKind resultKind = ResultKind.Conflict)
            => new(false, erorrMessage, resultKind);
        public static Result NotFound(string erorrMessage, ResultKind resultKind = ResultKind.NotFound)
            => new(false, erorrMessage, resultKind);
        public static Result Validation(string erorrMessage, ResultKind resultKind = ResultKind.ValidationError)
            => new(false, erorrMessage, resultKind);
    }
    public sealed record Result<T>(bool IsSuccess,T?value, string? errorMessage = null, ResultKind resultKind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true , value);
        public static Result<T> Fail(string erorrMessage, ResultKind resultKind = ResultKind.Conflict)
            => new(false,default, erorrMessage, resultKind);
        public static Result<T> NotFound(string erorrMessage, ResultKind resultKind = ResultKind.NotFound)
            => new(false, default, erorrMessage, resultKind);
    }
}
