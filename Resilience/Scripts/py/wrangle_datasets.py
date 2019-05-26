import pandas as pd
import sys

# Employment by region
a = pd.read_excel("../../App_Data/Employment by Industry.xlsx",sheet_name="SA4 & City Metro",usecols=[0,1,3])
a = a[a["State/Territory"]=="VIC"]
a = a.groupby("Region Name", as_index=False).agg({"Employment by Industry - Total":sum}).rename(columns={"Region Name":"Vic_region","Employment by Industry - Total":"Employment"})
a.to_csv("../../App_Data/Employment_region_VIC.txt", sep="\t",index=False)

# Employment forecast
b = pd.read_excel("../../App_Data/Industry Employment Projections.xlsx",skiprows=2,usecols=[0,2,3,4],names=["level","Industry","Employment_2018_000","Employment_2023_000"])
b = b[b["level"]==1]
b.iloc[:,1:].to_csv("../../App_Data/2018_Industry_Emplyoment_projections.txt", sep="\t",index=False)

# Unemployment trend
c = pd.read_excel("../../App_Data/Macroeconomic-Indicators.xlsx",sheet_name="Unemployment rate",skiprows=4,usecols=[0,1], names=["year","unemployment_rate"])
c = c.dropna()
c.year = pd.to_numeric(c.year.str[0:4],downcast='signed')+1
c.to_csv("../../App_Data/Historical_Victorian_unmployment.txt", sep="\t",index=False)
