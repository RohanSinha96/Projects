# Setting working directory
setwd("C:/Users/Rohan Sinha/Desktop/Mini Project")

#Reading Data
data<-read.csv("Close.csv")



library(h2o)
h2o.init(nthreads = -1)
# Using 1 Hidden Layer
model = h2o.deeplearning(y = 'Close',
                         training_frame = as.h2o(data),
                         activation = 'Tanh',
                         hidden = c(5,5,5),
                         epochs = 200,
                         train_samples_per_iteration = -2)



# Predicting the Test set results
y_pred = h2o.predict(model, newdata = as.h2o(data))
y_pred = as.vector(y_pred)
y_actual = as.vector(data[3])

# Calculating error percentage
error = abs((y_pred-data[3]))
error_percent = (error/(data[3]))*100
error = as.vector(error)
avg_error = sum(error_percent)/45



data1 <- data.frame(
  predicted = y_pred
)

new_data1 <- data.frame(
  Predicted1 = y_pred, 
  Actual = y_actual
)

regressor = lm(formula = Close ~ Predicted1,
               data = new_data1)
y_pred2 = predict(regressor, newdata = new_data1)

error2 = abs((y_pred2-new_data1[2]))
error_percent2 = (error2/(new_data1[2]))*100
error2 = as.vector(error2)
avg_error2 = sum(error_percent2)/45

data2 <- data.frame(
  predicted = y_pred2
)



regressor = lm(formula = Close ~ Open+X1,
               data = data)
y_pred3 = predict(regressor, newdata = data)

error3 = abs((y_pred3-data[3]))
error_percent3 = (error3/(data[3]))*100
error3 = as.vector(error3)
avg_error3 = sum(error_percent3)/45

data3 <- data.frame(
  predicted = y_pred3
)

m<-read.csv("data(With Clustering).csv")

table(m[9])
table(m[15])
table(m[21])
