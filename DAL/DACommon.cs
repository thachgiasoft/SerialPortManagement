using System;
using System.Collections.Generic;
using System.Linq;
using ComManagement.Bo;
using ComManagement.Dto;
namespace ComManagement.DAL
{
    public class DACommon
    {
        private DataSet db;



        public DACommon()
        {
            db = new DataSet();
        }
        #region Port
        public bool InsertPort(ComPortSetting port)
        {
            db.tbPorts.AddtbPortsRow(port.PortName, port.DataBits, port.BaudRate, (int)port.StopBits, port.Parity.ToString(), port.Rts);
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool InsertPorts(List<ComPortSetting> ports)
        {
            foreach (var port in ports)
            {
                db.tbPorts.AddtbPortsRow(port.PortName, port.DataBits, port.BaudRate, int.Parse(port.StopBits.ToString()), port.Parity.ToString(), port.Rts);
            }
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdatePort(ComPortSetting port)
        {
            var tmp = db.tbPorts.FindByPortName(port.PortName);

            tmp.BaudRate = port.BaudRate;
            tmp.DataBit = port.DataBits;
            tmp.Dts = port.Rts;
            tmp.Parity = port.Parity.ToString();
            tmp.StopBit = int.Parse(port.StopBits.ToString());
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeletePortById(int id)
        {
            var row = db.tbPorts.FindByPortId(id);
            int index = db.tbPorts.Rows.IndexOf(row);
            db.tbPorts.Rows[index].Delete();
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Service
        public bool InsertSevice(ServiceDto service)
        {
            db.tbServiceRef.AddtbServiceRefRow(service.TestId, service.TestName);
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool InsertSevices(List<ServiceDto> services)
        {
            foreach (var service in services)
            {
                db.tbServiceRef.AddtbServiceRefRow(service.TestId, service.TestName);
            }
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateSevice(ServiceDto service)
        {
            var tmp = db.tbServiceRef.FindByTestId(service.Id);
            tmp.TestName = service.TestName;
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteSeviceById(int serviceId)
        {
            var row = db.tbServiceRef.FindByTestId(serviceId);
            int index = db.tbServiceRef.Rows.IndexOf(row);
            db.tbServiceRef.Rows[index].Delete();
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Service ref Patient
        public bool InsertServiceRefPatient(ServiceRefPatientDto refPatient)
        {
            db.tbServiceRef_Patients.AddtbServiceRef_PatientsRow(refPatient.Barcode,
                refPatient.TestId,refPatient.TestValue);
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool InsertServiceRefPatients(List<ServiceRefPatientDto> services)
        {
            foreach (var refPatient in services)
            {
                db.tbServiceRef_Patients.AddtbServiceRef_PatientsRow(refPatient.Barcode,
               refPatient.TestId, refPatient.TestValue);
            }
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateServiceRefPatient(ServiceRefPatientDto serfPatient)
        {
            var tmp = db.tbServiceRef_Patients.FindById(serfPatient.Id);
            tmp.Barcode = serfPatient.Barcode;
            tmp.TestValue = serfPatient.TestValue;
            tmp.IDTestName = serfPatient.TestId;
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteServiceRefPatientById(int id)
        {
            var row = db.tbServiceRef_Patients.FindById(id);
            int index = db.tbServiceRef_Patients.Rows.IndexOf(row);
            db.tbServiceRef_Patients.Rows[index].Delete();
            try
            {
                db.AcceptChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
