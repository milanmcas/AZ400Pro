﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using awsapi.Models;
using System.Net;
using System.Text.Json;

namespace awsapi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly string _tableName = "customers";//Table name we created in Dynamo DB

        public CustomerRepository(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async Task<bool> CreateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            var customerAsJson = JsonSerializer.Serialize(customer);
            var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = customerAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(createItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<Customer?> GetAsync(Guid id)
        {
            var getItemRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDB.GetItemAsync(getItemRequest);

            if (response.Item.Count == 0)
            {
                return null;
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);

            return JsonSerializer.Deserialize<Customer>(itemAsDocument.ToJson());
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var scanRequest = new ScanRequest
            {
                TableName = _tableName,
            };

            var response = await _dynamoDB.ScanAsync(scanRequest);

            return response.Items.Select(x =>
            {
                var json = Document.FromAttributeMap(x).ToJson();
                return JsonSerializer.Deserialize<Customer>(json);
            });
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            var customerAsJson = JsonSerializer.Serialize(customer);
            var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

            var updateItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = customerAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(updateItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
