﻿using System;
using System.Data;

using NUnit.Framework;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

namespace Tests.Data
{
	using System.Configuration;
	using LinqToDB.DataProvider.DB2iSeries;
	using Model;

	[TestFixture]
	public class DataConnectionTests : TestBase
	{
		[Test]
		public void Test2()
		{
			using (var conn = new DataConnection())
			{
				Assert.That(conn.Connection.State,    Is.EqualTo(ConnectionState.Open));
				Assert.That(conn.ConfigurationString, Is.EqualTo(DataConnection.DefaultConfiguration));
			}
		}

		[Test]
		public void EnumExecuteScalarTest()
		{
			using (var dbm = new DataConnection())
			{
				var gender = dbm.Execute<Gender>(string.Format("select 'M'{0}", TestBase.GetDummyFrom(dbm.DataProvider)));

				Assert.That(gender, Is.EqualTo(Gender.Male));
			}
		}

		[Test, DataContextSource(false)]
		public void CloneTest(string context)
		{
			using (var con = new DataConnection(context))
			{
				var dbName = con.Connection.Database;

				for (var i = 0; i < 150; i++)
					using (var clone = (DataConnection)con.Clone())
						dbName = clone.Connection.Database;
			}
		}
	}
}
