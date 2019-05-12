import pandas as pd
import json
import sys

a = pd.read_csv(sys.argv[1]+"App_Data/2018_Industry_Emplyoment_projections.txt", sep='\t')
b = pd.read_csv(sys.argv[1]+"App_Data/Employment_region_VIC.txt", sep='\t')
b = b.sort_values("Employment",ascending=True)
c = pd.read_csv(sys.argv[1]+"App_Data/Historical_Victorian_employment.txt", sep='\t')

datasets = {"Industry_empl_2018":a.to_dict(orient="records"),
            "Employment_region":b.to_dict(orient="records"),
            "Hist_Vic_empl":c.to_dict(orient="records")}

print(json.dumps(datasets))