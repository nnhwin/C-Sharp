using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRXMLThesis
{
    class BookInfo
    {
        public string authors,bkid, title, genre, publish_date, description, price, issn_number,publisher,publish_number_of_times;
        public string article, masterthesis, journal, volume, year, ee, cdrom;
        
        public BookInfo()
        {

        }
        public BookInfo(string authors,string bkid,string title,string genre,string publish_date,string description,string price,string issn_number,string publisher,string publish_number_of_times)
        {
            this.bkid = bkid;
            this.authors = authors;
            this.description = description;
            this.title = title;
            this.price = price;
            this.issn_number = issn_number;
            this.publisher = publisher;
            this.genre = genre;
            this.publish_date = publish_date;
            this.publish_number_of_times = publish_number_of_times;
        }

        public void setMasterThesis(string thesis)
        {
            this.masterthesis = thesis;
        }
        public string getMasterThesis()
        {
            return this.masterthesis;
        }
        public void setArticle(string article)
        {
            this.description = article;
        }
        public string getArticle()
        {
            return this.article;
        }

        public void setDescription(string description)
        {
            this.description = description;
        }
        public string getDescritpion()
        {
            return this.description;
        }
        public void setTitle(string title)
        {
            this.title = title;
        }
        public string getTitle()
        {
            return title;
        }

        public void setPushTime(string publish_number_of_times)
        {
            this.publish_number_of_times = publish_number_of_times;
        }
        public string getPushTime()
        {
            return this.publish_number_of_times;
        }
        public void setPublisher(string publisher)
        {
            this.publisher = publisher;
        }
        public string getPublisher()
        {
            return this.publisher;
        }

        public void setISSNNumber(string issnNumber)
        {
            this.issn_number = issnNumber;
        }
        public string getISSNNumber()
        {
            return this.issn_number;
        }

        public void setBkId(string bkid)
        {
            this.bkid = bkid;
        }
        public string getBkid()
        {
            return this.bkid;
        }
        public void setBkgenre(string bkgenre)
        {
            this.genre = bkgenre;
        }
        public string getBkgenre()
        {
            return this.genre;
        }
        public void setBkPublishDate(string pushDate)
        {
            this.publish_date = pushDate;
        }
        public string getPushDate()
        {
            return this.publish_date;
        }
        public void setPrice(string price)
        {
            this.price = price;
        }
        public string getPrice()
        {
            return this.price;
        }
        public string getAuthorList()
        {
            return this.authors;
        }
        public void setAuthorList(string au)
        {
            this.authors = au;
        }
    }
}
