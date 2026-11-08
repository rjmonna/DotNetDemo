using Azure;
using Azure.Data.Tables;

namespace DotNetDemo.Infrastructure
{
    public class DynamicTableEntity : Dictionary<string, object?>, ITableEntity
    {
        public string PartitionKey {
            get {
                if (!this.ContainsKey(nameof(PartitionKey)))
                {
                    this[nameof(PartitionKey)] = default(string);
                }

                return (string)this[nameof(PartitionKey)];
            }
            set {
                this[nameof(PartitionKey)] = value;
            }
         }

        public string RowKey {
            get {
                if (!this.ContainsKey(nameof(RowKey)))
                {
                    this[nameof(RowKey)] = default(string);
                }

                return (string)this[nameof(RowKey)];
            }
            set {
                this[nameof(RowKey)] = value;
            }
         }

        public DateTimeOffset? Timestamp {
            get {
                if (!this.ContainsKey(nameof(Timestamp)))
                {
                    this[nameof(Timestamp)] = default(DateTimeOffset?);
                }

                return (DateTimeOffset?)this[nameof(Timestamp)];
            }
            set {
                this[nameof(Timestamp)] = value;
            }
         }

        public ETag ETag {
            get {
                if (!this.ContainsKey(nameof(ETag)))
                {
                    this[nameof(ETag)] = default(ETag);
                }

                return (ETag)this[nameof(ETag)];
            }
            set {
                this[nameof(ETag)] = value;
            }
         }
    }
}