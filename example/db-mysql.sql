 

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for school
-- ----------------------------
DROP TABLE IF EXISTS `school`;
CREATE TABLE `school`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SchoolId` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

 -- ----------------------------
-- Table structure for student
-- ----------------------------
DROP TABLE IF EXISTS `student`;
CREATE TABLE `student`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `Type` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '类型',
  `IsDel` bit(2) NULL DEFAULT NULL COMMENT '是否删除',
  `Status` tinyint(4) NULL DEFAULT NULL COMMENT '状态',
  `LongValue` bigint(8) NULL DEFAULT NULL COMMENT 'LongValue值',
  `FloatValue` float NULL DEFAULT NULL COMMENT 'FloatValue值',
  `DecimalValue` decimal(16, 2) NULL DEFAULT NULL COMMENT 'DecimalValue值',
  `Body` varchar(4000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '内容',
  `DateCreated` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `SchoolId` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '学校ID',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;
 
