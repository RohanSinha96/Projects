# Importing the required packages
import pandas as pd
from rake_nltk import Rake
import numpy as np
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.feature_extraction.text import CountVectorizer


# Reading up the data
pd.set_option('display.max_columns', 100)
df = pd.read_csv('zomato.csv',nrows = 5000)


df = df[['name','rate','votes','location','rest_type','dish_liked','cuisines','listed_in(type)']]



# Trimming whitespace between words and replacing commas with whitespace for keywords
df['rest_type'] = df['rest_type'].str.replace(' ','')
df['rest_type'] = df['rest_type'].str.replace(',',' ')

df['dish_liked'] = df['dish_liked'].str.replace(' ','')
df['dish_liked'] = df['dish_liked'].str.replace(',',' ')

df['cuisines'] = df['cuisines'].str.replace(' ','')
df['cuisines'] = df['cuisines'].str.replace(',',' ')

words = df['rest_type'] + ' ' + df['dish_liked'] + ' ' + df['cuisines'] + ' ' + df['listed_in(type)']
df['Key_words'] = words

# Setting name of restaurant as index
df.set_index('name', inplace = True)

# Drop columns other than name, key words, location and rate 
df.drop(columns = [col for col in df.columns if col!= 'Key_words' and col!= 'location' and col!= 'rate'], inplace = True)

# Instantiating and generating the count matrix
count = CountVectorizer()
count_matrix = count.fit_transform(df['Key_words'].values.astype('U'))

# Creating a Series for the restaurants so they are associated to an ordered numerical
# List that will be used later to match the indexes
indices = pd.Series(df.index)
indices[:5]
cosine_sim = cosine_similarity(count_matrix, count_matrix)

# Function for recommending restaurants
def recommendations(name, cosine_sim = cosine_sim):
    
    recommended_rest = []
    
    # gettin the index of the restaurant that matches the title
    idx = indices[indices == name].index[0]

    # creating a Series with the similarity scores in descending order
    score_series = pd.Series(cosine_sim[idx]).sort_values(ascending = False)

    # getting the indexes of the 10 most similar restaurants
    top_10_indexes = list(score_series.iloc[1:20].index)
    
    #populating the list with the titles of the best 10 matching restaurant
    
    for i in top_10_indexes:
        if list(df.index)[i] != name:
           recommended_rest.append(list(df.index + ' | Location: ' + df['location'][i] + ' | Rating: ' + df['rate'])[i])
        
    return recommended_rest
	
recommendations('Samskruti - Sanman Gardenia')
