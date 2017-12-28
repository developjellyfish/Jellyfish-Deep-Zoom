using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using JellyfishAdmin.Common.Dao;
using JellyfishAdmin.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace JellyfishAdmin.CollectionManager
{
    /// <summary>
    /// CollectionManagerDao
    /// </summary>
    public class CollectionManagerDao : DaoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionManagerDao"/> class.
        /// </summary>
        public CollectionManagerDao()
        {

        }

        /// <summary>
        /// Gets the collection info by owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="cid">The cid.</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetCollectionInfoByOwner(String owner, String cid)
        {
            ArrayList entityList = new ArrayList();

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT CId, Url, Title, ApplyStartDate, ApplyEndDate, Date, Owner FROM collection_info"
                                + " WHERE Owner = @Owner";
                if (cid != null)
                {
                    cmdText += " AND CId = @CId";
                }
                cmdText += " ORDER BY CId DESC";

                Debug.WriteLine("EXEC_SQL : " + cmdText);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);
                if (cid != null)
                {
                    cmd.Parameters.AddWithValue("@CId", cid);
                }
   
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                CollectionInfoEntity entity = null;

                Hashtable ht = new Hashtable();
                while (reader.Read())
                {
                    entity = new CollectionInfoEntity();
                    entity.CId = (String)reader["CId"];
                    entity.Url = (String)reader["Url"];
                    entity.Title = (String)reader["Title"];
                    entity.ApplyStartDate = (DateTime)reader["ApplyStartDate"];
                    entity.ApplyEndDate = (DateTime)reader["ApplyEndDate"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];

                    entityList.Add(entity);
                    //Debug.WriteLine("RES_ROW : " + entity.ToString());
                }

                reader.Close();
            }
            return entityList;
        }

        /// <summary>
        /// Gets the upload info by C id.
        /// </summary>
        /// <param name="cid">The cid.</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetUploadInfoByCId(String cid)
        {
            ArrayList entityList = new ArrayList();

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT a.UId, a.Url, a.Width, a.Height, a.Thumbnail, a.Title, a.Description, a.Tags, a.IsShare, a.Date, a.Owner"
                                + " FROM upload_info a INNER JOIN collection_rel_info b ON a.UId = b.UId"
                                + " WHERE b.CId = @CId ORDER BY a.UId DESC";

                Debug.WriteLine("EXEC_SQL : " + cmdText);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@CId", cid);


                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                UploadInfoEntity entity = null;

                Hashtable ht = new Hashtable();
                while (reader.Read())
                {
                    entity = new UploadInfoEntity();
                    entity.UId = (String)reader["UId"];
                    entity.Url = (String)reader["Url"];
                    entity.Width = (int)reader["Width"];
                    entity.Height = (int)reader["Height"];
                    entity.Thumbnail = (String)reader["Thumbnail"];
                    entity.Title = (String)reader["Title"];
                    entity.Description = (String)reader["Description"];
                    entity.Tags = (String)reader["Tags"];
                    entity.IsShare = (int)reader["IsShare"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];

                    if (!ht.ContainsKey(entity.UId))
                    {
                        entityList.Add(entity);
                        ht.Add(entity.UId, "");
                    }


                    //Debug.WriteLine("RES_ROW : " + entity.ToString());
                }

                reader.Close();
            }
            return entityList;
        }

        /// <summary>
        /// Deletes the collection info by C id.
        /// </summary>
        /// <param name="targetCId">The target C id.</param>
        public void DeleteCollectionInfoByCId(ICollection targetCId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlTransaction sqlTran = conn.BeginTransaction();

                SqlCommand command = conn.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    string cmdText = null;
                    foreach (String cid in targetCId)
                    {
                        cmdText = "DELETE FROM collection_info WHERE CId = @CId";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", cid);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();

                        cmdText = "DELETE FROM collection_layout_info WHERE CId = @CId";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", cid);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();

                        cmdText = "DELETE FROM collection_rel_info WHERE CId = @CId";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", cid);
                        command.ExecuteNonQuery();
                    }
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DeleteCollectionInfoByCId Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }

        /// <summary>
        /// Creates the collection info.
        /// </summary>
        /// <param name="collectionInfo">The collection info.</param>
        /// <param name="targetUId">The target U id.</param>
        public void CreateCollectionInfo(CollectionInfoEntity collectionInfo, ICollection targetUId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlTransaction sqlTran = conn.BeginTransaction();

                SqlCommand command = conn.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    string cmdText = null;

                    cmdText = "INSERT INTO collection_info(CId, Url, Title, ApplyStartDate, ApplyEndDate, Date, Owner)"
                                + "VALUES(@CId, @Url, @Title, @ApplyStartDate, @ApplyEndDate, GETDATE(), @Owner)";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                    command.Parameters.AddWithValue("@Url", collectionInfo.Url);
                    command.Parameters.AddWithValue("@Title", collectionInfo.Title);
                    command.Parameters.AddWithValue("@ApplyStartDate", collectionInfo.ApplyStartDate);
                    command.Parameters.AddWithValue("@ApplyEndDate", collectionInfo.ApplyEndDate);
                    command.Parameters.AddWithValue("@Owner", collectionInfo.Owner);
                    command.ExecuteNonQuery();

                    cmdText = "INSERT INTO collection_rel_info(CId, UId, Date, Owner)"
                            + " VALUES(@CId, @UId, GETDATE(), @Owner)";

                    foreach (String uid in targetUId)
                    {
                        command.Parameters.Clear();
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                        command.Parameters.AddWithValue("@UId", uid);
                        command.Parameters.AddWithValue("@Owner", collectionInfo.Owner);
                        command.ExecuteNonQuery();
                    }

                    command.Parameters.Clear();
                    cmdText = "INSERT INTO layout_info(LId, Url, Title, Description, DefaultFlag, IsShare, Date, Owner)"
                                + "VALUES(@LId, @Url, @Title, @Description, @DefaultFlag, @IsShare, GETDATE(), @Owner)";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@LId", collectionInfo.CId + "_layout");
                    command.Parameters.AddWithValue("@Url", collectionInfo.Url);
                    command.Parameters.AddWithValue("@Title", "Default - Title");
                    command.Parameters.AddWithValue("@Description", "Created by System Default");
                    command.Parameters.AddWithValue("@DefaultFlag", "1");
                    command.Parameters.AddWithValue("@IsShare", "0");
                    command.Parameters.AddWithValue("@Owner", collectionInfo.Owner);
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    cmdText = "INSERT INTO collection_layout_info(CId, LId, Date, Owner)"
                                + "VALUES(@CId, @LId, GETDATE(), @Owner)";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                    command.Parameters.AddWithValue("@LId", collectionInfo.CId + "_layout");
                    command.Parameters.AddWithValue("@Owner", collectionInfo.Owner);
                    command.ExecuteNonQuery();

                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("CreateCollectionInfo Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }

        /// <summary>
        /// Updates the collection info.
        /// </summary>
        /// <param name="collectionInfo">The collection info.</param>
        /// <param name="targetUId">The target U id.</param>
        public void UpdateCollectionInfo(CollectionInfoEntity collectionInfo, ICollection targetUId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlTransaction sqlTran = conn.BeginTransaction();

                SqlCommand command = conn.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    string cmdText = null;

                    cmdText = "UPDATE collection_info SET Title = @Title, ApplyStartDate = @ApplyStartDate, ApplyEndDate = @ApplyEndDate, Date = GETDATE()"
                                + " WHERE CId = @CId";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@Title", collectionInfo.Title);
                    command.Parameters.AddWithValue("@ApplyStartDate", collectionInfo.ApplyStartDate);
                    command.Parameters.AddWithValue("@ApplyEndDate", collectionInfo.ApplyEndDate);
                    command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    cmdText = "DELETE FROM collection_rel_info WHERE CId = @CId";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                    command.ExecuteNonQuery();

                    cmdText = "INSERT INTO collection_rel_info(CId, UId, Date, Owner)"
                            + " VALUES(@CId, @UId, GETDATE(), @Owner)";
                    foreach (String uid in targetUId)
                    {
                        command.Parameters.Clear();
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", collectionInfo.CId);
                        command.Parameters.AddWithValue("@UId", uid);
                        command.Parameters.AddWithValue("@Owner", collectionInfo.Owner);
                        command.ExecuteNonQuery();
                    }

                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UpdateCollectionInfo Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }
    }
}