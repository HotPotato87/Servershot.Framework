﻿using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using ServerShot.Framework.Core.Implementation.Persistance;
using NUnit.Framework;

namespace ServerShot.Framework.Tests.IntegrationTests
{
    public class When_Using_Azure_Table_Persistance
    {
        #region Key

        public string _accountName = "serviceshot";
        public string _accountKey = "57Kn2DQl1nX/TjZQyNyNZzDuyfOup0fCfPoT/gZqdYJc8lpygJUq9S6lln3f5O7+s6PzBbArIn2Z+cqMcdqqDg==";
        public string tableName = "people";

        private AzureTablePersistance CreateTablePersistanceClient()
        {
            return new AzureTablePersistance(_accountName, _accountKey);
        }

        #endregion

        [Test]
        public async Task Storing_A_Value_And_Retrieving_A_Value_Works()
        {
            string key = "apple";
            bool value = true;

            AzureTablePersistance azureTable = CreateTablePersistanceClient();
            azureTable.TableName = tableName;
            await azureTable.StoreAsync(key, value);

            Assert.IsTrue(await azureTable.RetrieveAsync<bool>(key) == value);
        }

        [Test]
        [ExpectedException(typeof(InvalidCredentialException))]
        public async Task Bad_Key_Throws_Exception()
        {
            string key = "apple";
            bool value = true;

            AzureTablePersistance azureTable = new AzureTablePersistance("", "");
            azureTable.TableName = tableName;
            await azureTable.StoreAsync(key, value);
        }

        [Test]
        public async Task Items_Can_Be_Replaced()
        {
            string key = "apple";
            bool value = true;

            bool newValue = false;

            AzureTablePersistance azureTable = CreateTablePersistanceClient();
            azureTable.TableName = tableName;
            await azureTable.StoreAsync(key, value);

            Assert.IsTrue(await azureTable.RetrieveAsync<bool>(key) == value);

            await azureTable.StoreAsync(key, newValue);

            Assert.IsTrue(await azureTable.RetrieveAsync<bool>(key) == newValue);
        }
    }
}
