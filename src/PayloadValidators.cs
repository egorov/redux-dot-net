using System;
using System.Collections.Generic;

namespace Redux
{
    public class PayloadValidators
    {
        private Dictionary<string, HashSet<ValueValidator>> container;
        public PayloadValidators()
        {
            this.container = new Dictionary<string, HashSet<ValueValidator>>();
        }

        public IEnumerable<string> Keys {
            get{
                return this.container.Keys;
            }
        }

        public void Add(string key, ValueValidator validator)
        {
            this.ValidateKey(key);

            if(validator == null)
                throw new ArgumentNullException("validator");

            this.CreateValidatorsCollectionIfNotExists(key);
            this.TryToAddNewValidator(key, validator);
        }

        private void ValidateKey(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if(string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
        }

        private void CreateValidatorsCollectionIfNotExists(string key)
        {
            if(!this.container.ContainsKey(key))
                this.container.Add(key, new HashSet<ValueValidator>());
        }

        private void TryToAddNewValidator(string key, ValueValidator validator)
        {
            HashSet<ValueValidator> hashSet = this.container[key] as HashSet<ValueValidator>;
            hashSet.Add(validator);
        }

        public IEnumerable<ValueValidator> Get(string key)
        {
            if(this.container.ContainsKey(key))
                return this.container[key];

            return new HashSet<ValueValidator>();
        }
    }
}