#import nltk
import json
import sys
import re
import pandas as pd

# nltk.download('vader_lexicon')

def nltk_sentiment(sentence):
    from nltk.sentiment.vader import SentimentIntensityAnalyzer
    
    nltk_sentiment = SentimentIntensityAnalyzer()
    score = nltk_sentiment.polarity_scores(sentence)
    return score

def minmax_sentiment(text):
	text_sent = re.split("(?:\.{1,3}|!)[ \\n]",text)
	
	score = [(nltk_sentiment(x))["compound"] for x in text_sent]
	score_df = pd.DataFrame({"text":text_sent, "score":score})
	
	max_score = score_df.loc[(score_df["score"] >= 0.25) & (score_df["score"]==score_df["score"].max()),]
	min_score = score_df.loc[(score_df["score"] <= -0.25) & (score_df["score"]==score_df["score"].min()),]
	
	max_score["type"] = "max"
	min_score["type"] = "min"
	
	return pd.concat([max_score,min_score])

# Save text input from command call	
text = sys.argv[1]
#text = "Food is good and not too expensive... Serving is just right for adult.. Ambient is nice too. Used to be good. Chicken soup was below average, bbq used to be good. Food was good, standouts were the spicy beef soup and seafood pancake! Good office lunch or after work place to go to with a big group as they have a lot of private areas with large tables. As a Korean person, it was very disappointing food quality and very pricey for what you get. I won't go back there again. Not great quality food for the price. Average food at premium prices really. Fast service. Prices are reasonable and food is decent. Extremely long waiting time. Food is decent but definitely not worth the wait. Clean premises, tasty food. My family favourites are the clear Tom yum soup, stuffed chicken wings, chargrilled squid. really good and authentic Thai food! in particular, we loved their tom yup clear soup with sliced fish. it's so well balanced that we can have it just on its own."

# Get overall result and attach text
res = minmax_sentiment(text).to_dict(orient="records")

# Return json response
print(json.dumps(res))