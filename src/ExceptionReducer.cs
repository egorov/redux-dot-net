using System.Collections.Generic;

namespace Redux
{
    public class ExceptionReducer : ReducerArgumentsValidator, Reducer
    {

        private readonly string type;
        public ExceptionReducer()
        {
            this.type = "Exception";
        }

        public IDictionary<string, object> Reduce(IDictionary<string, object> state, Message message)
        {
            base.ValidateState(state);
            base.ValidateMessage(message);

            if (message.Type != this.type)
                return state;

            IDictionary<string, object> after = new Dictionary<string, object>(state);

            List<object> errors = this.GetErrorsContainer(after);

            errors.Add(message.Payload);

            return after;
        }

        private List<object> GetErrorsContainer(IDictionary<string, object> state)
        {

            if (state.ContainsKey(this.type))
                return state[this.type] as List<object>;

            List<object> errors = new List<object>();

            state.Add(this.type, errors);

            return errors;
        }
    }
}