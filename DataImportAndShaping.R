library(tidyverse)
df1 <- read_csv("MTA20834 Semester Project/questionnaires results/Survey after both conditions.csv")
df2 <- read_csv("MTA20834 Semester Project/questionnaires results/Survey after each condition.csv")
allData <- read_csv("MTA20834 Semester Project/logFiles/allData.csv")

df1 <- setNames(df1, c("Time", "PID", "PC_Discrete", "PC_Continuous", "CC_Overall", "ReasonsOther", "Comments", "preferCond"))
df2 <- setNames(df2, c("Time", "PID", "condition", "PC_KS", "PC_Fish"))

df1 <- df1 %>%
  select(c(2:5)) %>%
  pivot_longer(-PID, names_sep = "_", names_to = c("rating", "condition"), values_to = "value") %>%
  mutate(context = "All")

df2 <- df2 %>%
  select(c(2:5)) %>%
  pivot_longer(-c(PID, condition), names_sep = "_", names_to = c("rating", "context"), values_to = "value")

df <- rbind(df1, df2)
df1 <- NULL
df2 <- NULL

allData <- allData %>%
  select(c(3:5, "TotalFishCaught", "BubbleNumber", Event, GlobalFrustration, InstantFrustration)) %>%
  rename(Global_F = GlobalFrustration, Instant_F = InstantFrustration, condition = currentCondition, PID = Participant) %>%
  filter(Event == "Frustration Questionnaire")

cor(allData$Global_F, allData$Instant_F)
# what does the above plot indicate?
ConditionOrder <- allData %>%
  select(PID, ConditionOrder) %>%
  group_by(PID, ConditionOrder) %>%
  summarize()

dfAgg <- allData %>%
  group_by(PID, condition) %>%
  summarize(avgG_F = mean(Global_F), avgI_F = mean(Instant_F))

dfAgg <- dfAgg %>% pivot_longer(-c(PID, condition), names_sep = "_", names_to = c("context", "rating"), values_to = "value")
dfAgg <- dfAgg[, c(1, 4, 2, 5, 3)]
dfAll <- rbind(data.frame(dfAgg), data.frame(df))

dfAllWide <- dfAll %>% pivot_wider(names_from = c(rating, context), values_from = value)
dfAllWide <- dfAllWide %>%
  select(-CC_All) %>%
  filter(!condition == "Overall")

ggplot(dfAllWide, aes(x = PC_All, y = F_avgI, colour = condition)) +
  geom_point() +
  geom_smooth(method = lm, se = FALSE) +
  theme_bw()
ggplot(dfAllWide, aes(x = PC_All, y = F_avgI, colour = condition)) +
  geom_point() +
  geom_smooth(method = lm, se = FALSE) +
  theme_bw()
ggplot(dfAllWide, aes(x = condition, y = PC_All)) +
  geom_boxplot(outlier.shape = NA) +
  geom_jitter(width = 0.1, colour = "red") +
  theme_bw()
ggplot(dfAllWide, aes(x = condition, y = F_avgI)) +
  geom_boxplot(outlier.shape = NA) +
  geom_jitter(width = 0.1, colour = "red") +
  theme_bw()

plot(dfAllWide$F_avgG, dfAllWide$F_avgI)
cor(dfAllWide$F_avgG, dfAllWide$F_avgI)
qqnorm(dfAllWide$F_avgG)
qqline(dfAllWide$F_avgG)

qqnorm(dfAllWide$F_avgI)
qqline(dfAllWide$F_avgI)
