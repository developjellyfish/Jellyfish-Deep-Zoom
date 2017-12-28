using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using JellyfishAdmin.Common.Dao;
using JellyfishAdmin.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace JellyfishAdmin.LayoutManager
{
    /// <summary>
    /// LayoutManagerDao
    /// </summary>
    public class LayoutManagerDao : DaoBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutManagerDao()
        {

        }

        /// <summary>
        /// Layout Info Entity
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="lid">Layout ID</param>
        /// <returns>LayoutInfoEntity Object</returns>
        public LayoutInfoEntity GetLayoutInfoByLId(String owner, String lid)
        {
            LayoutInfoEntity entity = null;

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT LId, Url, Title, Description, DefaultFlag, IsShare, Date, Owner FROM layout_info"
                                + " WHERE Owner = @Owner AND LId = @LId";

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);
                cmd.Parameters.AddWithValue("@LId", lid);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    entity = new LayoutInfoEntity();
                    entity.LId = (String)reader["LId"];
                    entity.Url = (String)reader["Url"];
                    entity.Title = (String)reader["Title"];
                    entity.Description = System.Convert.ToString(reader["Description"]);
                    entity.DefaultFlag = (Int32)reader["DefaultFlag"];
                    entity.IsShare = (int)reader["IsShare"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];
                }
                reader.Close();
            }
            return entity;
        }

        /// <summary>
        /// Get Collection Layout Info By Collection ID
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="Cid">Collection ID</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetCollectionLayoutInfoByCId(String owner, String Cid)
        {
            ArrayList entityList = new ArrayList();
            CollectionLayoutInfoEntity entity = null;

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT CId, LId, Date, Owner FROM collection_layout_info"
                                + " WHERE Owner = @Owner AND Cid = @Cid";

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);
                cmd.Parameters.AddWithValue("@Cid", Cid);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    entity = new CollectionLayoutInfoEntity();
                    entity.CId = (String)reader["CId"];
                    entity.LId = (String)reader["LId"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];

                    entityList.Add(entity);
                }
                reader.Close();
            }
            return entityList;
        }

        /// <summary>
        /// Get Layout Info By Keyword
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="title">Title</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetLayoutInfoByKeyword(String owner, String title)
        {
            ArrayList entityList = new ArrayList();

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT a.LId, a.Url, a.Title, a.Description, a.DefaultFlag, a.IsShare, a.Date, a.Owner"
                                + " FROM layout_info a"
                                + " WHERE a.Owner = @Owner"
                                + " AND a.DefaultFlag = 0";
                if (title != null)
                {
                    cmdText += " AND UPPER(a.Title) LIKE '%" + title.Trim().ToUpper() + "%'";
                }

                cmdText += " UNION ALL(SELECT a.LId, a.Url, a.Title, a.Description, a.DefaultFlag, a.IsShare, a.Date, a.Owner"
                                + " FROM layout_info a"
                                + " WHERE a.IsShare = 1"
                                + " AND a.DefaultFlag = 0";
                if (title != null)
                {
                    cmdText += " AND UPPER(a.Title) LIKE '%" + title.Trim().ToUpper() + "%'";
                }

                cmdText += ") ORDER BY a.LId DESC";


                Debug.WriteLine("EXEC_SQL : " + cmdText);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);

   
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                LayoutInfoEntity entity = null;

                Hashtable ht = new Hashtable();
                while (reader.Read())
                {
                    entity = new LayoutInfoEntity();
                    entity.LId = (String)reader["LId"];
                    entity.Url = (String)reader["Url"];
                    entity.Title = (String)reader["Title"];
                    entity.Description = System.Convert.ToString(reader["Description"]);
                    entity.DefaultFlag = (Int32)reader["DefaultFlag"];
                    entity.IsShare = (int)reader["IsShare"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];

                    if (!ht.ContainsKey(entity.LId))
                    {
                        entityList.Add(entity);
                        ht.Add(entity.LId, "");
                    }
                    

                    //Debug.WriteLine("RES_ROW : " + entity.ToString());
                }

                reader.Close();
            }
            return entityList;
        }

        /// <summary>
        /// Get Layout Info of Only Mine By Keyword
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="title">Title</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetLayoutInfoOnlyMineByKeyword(String owner, String title)
        {
            ArrayList entityList = new ArrayList();

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT a.LId, a.Url, a.Title, a.Description, a.DefaultFlag, a.IsShare, a.Date, a.Owner"
                                + " FROM layout_info a"
                                + " WHERE a.Owner = @Owner"
                                + " AND a.DefaultFlag = 0";
                if (title != null)
                {
                    cmdText += " AND UPPER(a.Title) LIKE '%" + title.Trim().ToUpper() + "%'";
                }

                cmdText += " ORDER BY a.LId DESC";


                Debug.WriteLine("EXEC_SQL : " + cmdText);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);


                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                LayoutInfoEntity entity = null;

                Hashtable ht = new Hashtable();
                while (reader.Read())
                {
                    entity = new LayoutInfoEntity();
                    entity.LId = (String)reader["LId"];
                    entity.Url = (String)reader["Url"];
                    entity.Title = (String)reader["Title"];
                    entity.Description = System.Convert.ToString(reader["Description"]);
                    entity.DefaultFlag = (Int32)reader["DefaultFlag"];
                    entity.IsShare = (int)reader["IsShare"];
                    entity.Date = (DateTime)reader["Date"];
                    entity.Owner = (String)reader["Owner"];

                    if (!ht.ContainsKey(entity.LId))
                    {
                        entityList.Add(entity);
                        ht.Add(entity.LId, "");
                    }


                    //Debug.WriteLine("RES_ROW : " + entity.ToString());
                }

                reader.Close();
            }
            return entityList;
        }


        /// <summary>
        /// Update Layout Info By Entity
        /// </summary>
        /// <param name="entity">LayoutInfoEntity Object</param>
        public void UpdateLayoutInfoByEntity(LayoutInfoEntity entity)
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

                    cmdText = "UPDATE layout_info SET Title = @Title, Description = @Description, IsShare = @IsShare"
                              + " WHERE LId = @LId";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@Title", entity.Title);
                    command.Parameters.AddWithValue("@Description", entity.Description);
                    command.Parameters.AddWithValue("@IsShare", entity.IsShare);
                    command.Parameters.AddWithValue("@LId", entity.LId);
                    command.ExecuteNonQuery();
                   
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UpdateLayoutInfoByEntity Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }

        /// <summary>
        /// Delete Layout Info By Layout ID
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="targetLId">Layout ID</param>
        public void DeleteLayoutInfoByLId(String owner, ICollection targetLId)
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
                    foreach (String lid in targetLId)
                    {
                        cmdText = "DELETE FROM layout_info WHERE LId = @LId AND Owner = @Owner";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@LId", lid);
                        command.Parameters.AddWithValue("@Owner", owner);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();

                        cmdText = "DELETE FROM collection_layout_info WHERE LId = @LId";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@LId", lid);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();
                    }
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DeleteLayoutInfoByLId Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }

        /// <summary>
        /// Update Collection Layout Info
        /// </summary>
        /// <param name="cid">Collection ID</param>
        /// <param name="owner">Owner</param>
        /// <param name="targetLId">Layout ID</param>
        public void UpdateCollectionLayoutInfo(String cid, String owner, ICollection targetLId)
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

                    cmdText = "DELETE FROM collection_layout_info WHERE CId = @CId AND LId in (SELECT LId from layout_info WHERE CId = @CId AND DefaultFlag = 0)";
                    
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@CId", cid);
                    command.ExecuteNonQuery();

                    cmdText = "INSERT INTO collection_layout_info (CId, LId, Date, Owner)"
                            + " VALUES(@CId, @LId, GETDATE(), @Owner)";
                    foreach (String lid in targetLId)
                    {
                        command.Parameters.Clear();
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@CId", cid);
                        command.Parameters.AddWithValue("@LId", lid);
                        command.Parameters.AddWithValue("@Owner", owner);
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
