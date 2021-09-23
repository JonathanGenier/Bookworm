using System;

namespace BookWorm.Models.VO
{
    public class Book
    {
        private int _id;
        private string _author;
        private string _publisher;
        private string _genre;
        private string _title;
        private string _description;
        private string _imagePath;
        private string _publishedDate;
        private double _price;
        private int _numberOfPages;
        private int _stockQuantity;
        private int _soldQuantity;

        public Book() { }

        public Book(int id, string author, string publisher, 
            string genre, string title, string description, 
            string imagePath, string publishedDate, float price,
            int numberOfPages, int stockQuantity, int soldQuantity)
        {
            Id = id;
            Author = author;
            Publisher = publisher;
            Genre = genre;
            Title = title;
            Description = description;
            ImagePath = imagePath;
            PublishedDate = publishedDate;
            Price = price;
            NumberOfPages = numberOfPages;
            StockQuantity = stockQuantity;
            SoldQuantity = soldQuantity;
        }

        public int Id { get => _id; set => _id = value; }
        public string Author { get => _author; set => _author = value; }
        public string Publisher { get => _publisher; set => _publisher = value; }
        public string Genre { get => _genre; set => _genre = value; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public string ImagePath { get => _imagePath; set => _imagePath = value; }
        public string PublishedDate { get => _publishedDate; set => _publishedDate = value; }
        public double Price { get => _price; set => _price = value; }
        public int NumberOfPages { get => _numberOfPages; set => _numberOfPages = value; }
        public int StockQuantity { get => _stockQuantity; set => _stockQuantity = value; }
        public int SoldQuantity { get => _soldQuantity; set => _soldQuantity = value; }

        public override string ToString()
        {
            return String.Format
                (   "ID: {0}, Author: {1}, Publisher: {2}, Genre: {3}, Title: {4}, Description: {5}," +
                    " ImagePath: {6}, PublishedDate: {7}, Price: ${8}, NumberOfPages: {9}, StockQuantity: {10}, SoldQuantity: {11}",
                    Id.ToString(), 
                    Author, 
                    Publisher, 
                    Genre, 
                    Title, 
                    Description, 
                    ImagePath, 
                    PublishedDate, 
                    Price.ToString(), 
                    NumberOfPages.ToString(), 
                    StockQuantity.ToString(),
                    SoldQuantity.ToString()
                );
        }
    }
}
