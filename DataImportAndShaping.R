library(tidyverse)
df1 <- read_csv("MTA20834 Semester Project/questionnaires results/Survey after both conditions.csv")
df2 <- read_csv("MTA20834 Semester Project/questionnaires results/Survey after each condition.csv")
allData <- read_csv("MTA20834 Semester Project/logFiles/allData.csv")

df1 <- setNames(df1, c("Time", "PID", "PC_Discrete", "PC_Continuous", "CC", "ReasonsOther", "Comments", "preferCond"))
df2 <- setNames(df2, c("Time", "PID", "condition", "PC_KS", "PC_Fish"))

df1 <- df1 %>%
  select(c(2:5)) %>%
  pivot_longer(-PID, names_sep = "_", names_to = c("rating", "condition"), values_to = "value") %>%
  mutate(context = "All")

df2 <- df2 %>%
  select(c(2:5)) %>%
  pivot_longer(-c(PID, condition), names_sep = "_", names_to = c("rating", "context"), values_to = "value")

df <- rbind(df1, df2)

allData <- allData %>%
  select(c(3:5, "TotalFishCaught", "BubbleNumber", Event, GlobalFrustration, InstantFrustration)) %>%
  rename(Global_F = GlobalFrustration, Instant_F = InstantFrustration, condition = currentCondition,PID=Participant) %>%
  filter(Event == "Frustration Questionnaire") 

cor(allData$Global_F,allData$Instant_F)
#what does the above plot indicate?

dfAgg<-allData  %>% group_by(PID,ConditionOrder,condition) %>% summarize(avgG_F=mean(Global_F),avgI_F=mean(Instant_F))
