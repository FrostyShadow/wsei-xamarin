using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace AirMonitor.Models
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<InstallationEntity>().Wait();
            _database.CreateTableAsync<MeasurementEntity>().Wait();
            _database.CreateTableAsync<MeasurementItemEntity>().Wait();
            _database.CreateTableAsync<Value>().Wait();
            _database.CreateTableAsync<Index>().Wait();
            _database.CreateTableAsync<Standard>().Wait();
        }


        public async Task SaveAsync(IList<Installation> installationList)
        {
            foreach (var installation in installationList)
            {
                var entity = new InstallationEntity(installation);
                await _database.RunInTransactionAsync(db =>
                 {
                     db.Table<InstallationEntity>().Delete(c => true);
                     db.Insert(entity);
                 });
            }
        }

        public async Task SaveAsync(IList<Measurements> measurementList)
        {
            foreach (var measurement in measurementList)
            {
                var entity = new MeasurementEntity(measurement);
                await _database.RunInTransactionAsync(db =>
                {
                    db.Table<MeasurementEntity>().Delete(c => true);
                    db.Table<MeasurementItemEntity>().Delete(c => true);
                    db.Table<Value>().Delete(c => true);
                    db.Table<Index>().Delete(c => true);
                    db.Table<Standard>().Delete(c => true);

                    db.Insert(entity);
                    db.Insert(new MeasurementItemEntity(measurement.Current));
                    db.InsertAll(measurement.Current.Indexes, false);
                    db.InsertAll(measurement.Current.Standards, false);
                    db.InsertAll(measurement.Current.Values, false);
                });
            }
        }

        public async Task<IList<Installation>> GetInstallationsAsync()
        {
            var installationEntities = await _database.Table<InstallationEntity>().ToListAsync();
            return installationEntities.Select(installation => new Installation(installation)).ToList();
        }

        public async Task<Installation> GetInstallationAsync(int id)
        {
            var installationEntity = await _database.Table<InstallationEntity>().FirstOrDefaultAsync(i => i.Id == id);
            return new Installation(installationEntity);
        }

        public async Task<IList<Measurements>> GetMeasurementsAsync()
        {
            var measurementEntities = await _database.Table<MeasurementEntity>().ToListAsync();
            return measurementEntities.Select(measurement => new Measurements(measurement)).ToList();
        }

        public async Task<Measurements> GetMeasurementAsync(int id)
        {
            var measurementEntity = await _database.Table<MeasurementEntity>().FirstOrDefaultAsync(i => i.Id == id);
            return new Measurements(measurementEntity);
        }

        public async Task<IList<AveragedValues>> GetCurrentMeasurementsAsync()
        {
            var measurementItemEntities = await _database.Table<MeasurementItemEntity>().ToListAsync();
            return measurementItemEntities.Select(measurementItem => new AveragedValues(measurementItem)).ToList();
        }

        public async Task<AveragedValues> GetCurrentMeasurementAsync(int id)
        {
            var currentMeasurementEntity = await _database.Table<MeasurementItemEntity>().FirstOrDefaultAsync(i => i.Id == id);
            return new AveragedValues(currentMeasurementEntity);
        }
    }
}