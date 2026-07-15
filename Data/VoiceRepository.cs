using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Genration.Models;

namespace Genration.Data
{
    public class VoiceRepository
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["VoiceMailConnection"].ConnectionString;

        public List<VoiceMessage> GetVoiceMessages()
        {
            List<VoiceMessage> list = new List<VoiceMessage>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("GetVoiceMail", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    VoiceMessage vm = new VoiceMessage();

                    vm.Id = Convert.ToInt32(reader["Id"]);
                    vm.PhoneNumber = reader["PhoneNumber"].ToString();
                    vm.CallDate = Convert.ToDateTime(reader["CallDate"]);

                    vm.VoicePath = reader["VoicePath"] == DBNull.Value
                        ? ""
                        : reader["VoicePath"].ToString();

                    vm.RefrenceCode = reader["RefrenceCode"] == DBNull.Value
                        ? ""
                        : reader["RefrenceCode"].ToString();

                    vm.ReplyVoicePath = reader["ReplyVoicePath"] == DBNull.Value
                        ? ""
                        : reader["ReplyVoicePath"].ToString();

                    list.Add(vm);
                }
            }

            return list;
        }

        public void UpdateReplyVoice(string referenceCode, string fileName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UpdateReplyVoice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReferenceCode", referenceCode);
                cmd.Parameters.AddWithValue("@ReplyVoicePath", fileName);

                cmd.ExecuteNonQuery();
            }
        }
    }
}