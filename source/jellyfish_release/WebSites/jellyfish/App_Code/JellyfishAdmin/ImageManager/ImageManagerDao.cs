using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using JellyfishAdmin.Common.Dao;
using JellyfishAdmin.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace JellyfishAdmin.ImageManager
{

    /// <summary>
    /// ImageManagerDao Class
    /// </summary>
    public class ImageManagerDao : DaoBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageManagerDao()
        {

        }

        /// <summary>
        /// Get Upload Info By Unique ID
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="uid">Unique ID</param>
        /// <returns>UploadInfoEntity Object</returns>
        public UploadInfoEntity GetUploadInfoByUId(String owner, String uid)
        {
            UploadInfoEntity entity = null;

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT UId, Url, Width, Height, Thumbnail, Title, Description, Tags, IsShare, Date, Owner FROM upload_info"
                                + " WHERE Owner = @Owner AND UId = @UId";

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);
                cmd.Parameters.AddWithValue("@UId", uid);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
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
                }
                reader.Close();
            }
            return entity;
        }

        /// <summary>
        /// Get Upload Info By Keyword
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="title">Title</param>
        /// <param name="tags">Tags</param>
        /// <returns>ArrayList Object</returns>
        public ArrayList GetUploadInfoByKeyword(String owner, String title, string[] tags)
        {
            ArrayList entityList = new ArrayList();

            using (SqlConnection conn = GetConnection())
            {
                string cmdText = "SELECT a.UId, a.Url, a.Width, a.Height, a.Thumbnail, a.Title, a.Description, a.Tags, a.IsShare, a.Date, a.Owner, b.Tag"
                                + " FROM upload_info a LEFT JOIN tag_info b ON a.UId = b.UId"
                                + " WHERE a.Owner = @Owner";
                if (title != null)
                {
                    cmdText += " AND UPPER(a.Title) LIKE '%" + title.Trim().ToUpper() + "%'";
                }

                if (tags != null && tags.Length > 0)
                {
                    String tagCnd = "";
                    foreach (String tag in tags)
                    {
                        if (!"".Equals(tagCnd))
                        {
                            tagCnd += " OR ";
                        }
                        tagCnd += "UPPER(b.Tag) LIKE '%" + tag.Trim().ToUpper() + "%'";
                    }
                    cmdText += " AND (" + tagCnd + ")";
                }
                //cmdText += " ORDER BY a.UId DESC";


                cmdText += " UNION ALL(SELECT a.UId, a.Url, a.Width, a.Height, a.Thumbnail, a.Title, a.Description, a.Tags, a.IsShare, a.Date, a.Owner, b.Tag"
                                + " FROM upload_info a LEFT JOIN tag_info b ON a.UId = b.UId"
                                + " WHERE a.IsShare = 1";
                if (title != null)
                {
                    cmdText += " AND UPPER(a.Title) LIKE '%" + title.Trim().ToUpper() + "%'";
                }

                if (tags != null && tags.Length > 0)
                {
                    String tagCnd = "";
                    foreach (String tag in tags)
                    {
                        if (!"".Equals(tagCnd))
                        {
                            tagCnd += " OR ";
                        }
                        tagCnd += "UPPER(b.Tag) LIKE '%" + tag.Trim().ToUpper() + "%'";
                    }
                    cmdText += " AND (" + tagCnd + ")";
                }
                cmdText += ") ORDER BY a.UId DESC";


                Debug.WriteLine("EXEC_SQL : " + cmdText);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Owner", owner);

   
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

                    if (!ht.ContainsKey(entity.UId)) {
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
        /// Update Upload Info By Entity
        /// </summary>
        /// <param name="entity">UploadInfoEntity Object</param>
        public void UpdateUploadInfoByEntity(UploadInfoEntity entity)
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

                    cmdText = "DELETE FROM tag_info WHERE UId = @UId";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@UId", entity.UId);
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    cmdText = "UPDATE upload_info SET Title = @Title, Description = @Description, Tags = @Tags, IsShare = @IsShare"
                              + " WHERE UId = @UId";
                    command.CommandText = cmdText;
                    command.Parameters.AddWithValue("@Title", entity.Title);
                    command.Parameters.AddWithValue("@Description", entity.Description);
                    command.Parameters.AddWithValue("@Tags", entity.Tags);
                    command.Parameters.AddWithValue("@IsShare", entity.IsShare);
                    command.Parameters.AddWithValue("@UId", entity.UId);
                    command.ExecuteNonQuery();
                   
                    string[] tags = null;
                    if (entity.Tags != null && entity.Tags.Trim().Length > 0)
                    {
                        char[] delim = { ' ', ',', '.', ':', '\t' };
                        tags = entity.Tags.Split(delim);
                    }

                    if (tags != null)
                    {
                        cmdText = "INSERT INTO tag_info(UId, Tag, Date, Owner)";
                        cmdText += " VALUES(@UId, @Tag, GETDATE(), @Owner)";


                        foreach (String tag in tags)
                        {
                            command.Parameters.Clear();
                            command.CommandText = cmdText;
                            command.Parameters.AddWithValue("@UId", entity.UId);
                            command.Parameters.AddWithValue("@Tag", tag);
                            command.Parameters.AddWithValue("@Owner", entity.Owner);
                            command.ExecuteNonQuery();
                        }
                    }

                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UpdateUploadInfoByEntity Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }

        /// <summary>
        /// Delete Upload Info By Unique ID
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="targetUId">Unique ID</param>
        public void DeleteUploadInfoByUId(String owner, ICollection targetUId)
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
                    foreach (String uid in targetUId)
                    {
                        cmdText = "DELETE FROM upload_info WHERE UId = @UId AND Owner = @Owner";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@UId", uid);
                        command.Parameters.AddWithValue("@Owner", owner);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();

                        cmdText = "DELETE FROM tag_info WHERE UId = @UId AND Owner = @Owner";
                        command.CommandText = cmdText;
                        command.Parameters.AddWithValue("@UId", uid);
                        command.Parameters.AddWithValue("@Owner", owner);
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();
                    }
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DeleteUploadInfoByUId Error : " + ex.Message);
                    sqlTran.Rollback();
                }
            }
        }
    }
}
