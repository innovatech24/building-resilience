#import nltk
import sys

#nltk.download('vader_lexicon')

def nltk_sentiment(sentence):
    from nltk.sentiment.vader import SentimentIntensityAnalyzer
    
    nltk_sentiment = SentimentIntensityAnalyzer()
    score = nltk_sentiment.polarity_scores(sentence)
    return score

# Save text input from command call	
text = sys.argv[1]

# Get overall result and attach text
res = nltk_sentiment(text)
#res["text"] = text

# Return json response
print(res["compound"])
