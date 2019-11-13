# Multiple Linear Regression

# Setting working directory
setwd("C:/Users/Rohan Sinha/Desktop/Mini Project")

#Reading Data
data<-read.csv("Sensex.csv")

#Converting String to Integer
data$Date = substr(data$Date,start = 1,stop = 2) 
data$Date = strtoi(data$Date,base = 10L)


#Splitting data into test set and training set
library(caTools)
set.seed(123)
split = sample.split(data$Price, SplitRatio = 0.8)
training_set = subset(data, split == TRUE)
test_set = subset(data, split == FALSE)

# Feature Scaling
training_set[-7] = scale(training_set[-7])
test_set[-7] = scale(test_set[-7])


# Fitting Multiple Linear Regression to the Training set
regressor = lm(formula = Price ~ .,
               data = training_set)
summary(regressor)


regressor = lm(formula = Price ~ Date + High + Low + Close + Volume,
               data = training_set)
summary(regressor)


regressor = lm(formula = Price ~ Date + High + Close + Volume,
               data = training_set)
summary(regressor)


regressor = lm(formula = Price ~ Date + Close + Volume,
               data = training_set)
summary(regressor)


# Predicting the Test set results
y_pred = predict(regressor, newdata = test_set)
y_pred = as.vector(y_pred)
y_actual = as.vector(test_set[7])

error = abs((y_pred-test_set[7]))
error_percent = (error/(test_set[7]))*100

avg_error = sum(error_percent)/5

library(rsparkling)

library(dplyr)

library(ggplot2)

data1 <- data.frame(
  predicted = y_pred,
  actual    = y_actual
)
ggplot(data1, aes(x = y_actual, y = y_pred)) +
  geom_abline(slope = 1, col = "red") +
  geom_point() +
  theme(plot.title = element_text(hjust = 0.5)) +
  coord_fixed(ratio = 1) +
  labs(
    x = "Actual Stock Value",
    y = "Predicted Stock Value",
    title = "Predicted vs. Actual Stock Value"
  )

new_data1 <- data.frame(
  Date = "23-09-2017", 
  Open = 6205.05,
  High = 6445.80,
  Low = 6132.12,
  Close = 6777.05,
  Volume = 1963520000
)
new_data1$Date = substr(new_data1$Date,start = 1,stop = 2) 
new_data1$Date = strtoi(new_data1$Date,base = 10L)

price = predict(regressor, newdata = new_data1)
