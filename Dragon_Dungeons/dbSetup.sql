CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) DEFAULT CHARSET utf8 COMMENT '';

CREATE TABLE characters(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  LEVEL TINYINT DEFAULT 1,
  class VARCHAR(20) NOT NULL,
  race VARCHAR(20) NOT NULL,
  alignment ENUM(
    'Chaotic Evil',
    'Chaotic Good',
    'Chaotic Neutral',
    'Lawful Evil',
    'Lawful Good',
    'Lawful Neutral',
    'Neutral',
    'Neutral Evil',
    'Neutral Good'
  ) NOT NULL,
  age SMALLINT,
  feet TINYINT,
  inches TINYINT,
  weight SMALLINT,
  eyes VARCHAR(30),
  skin VARCHAR(30),
  hair VARCHAR(30),
  features VARCHAR(1000),
  background VARCHAR(100),
  backstory VARCHAR(1000),
  personalityTraits VARCHAR(500),
  ideals VARCHAR(500),
  bonds VARCHAR(500),
  flaws VARCHAR(500),
  manual BOOLEAN DEFAULT false,
  str TINYINT,
  dex TINYINT,
  con TINYINT,
  intelligence TINYINT,
  wis TINYINT,
  cha TINYINT,
  skills TEXT,
  proficiencies TEXT,
  cantrips TEXT,
  equipment TEXT,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id)
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE characters;

CREATE TABLE bonus(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  str TINYINT,
  dex TINYINT,
  con TINYINT,
  intelligence TINYINT,
  wis TINYINT,
  cha TINYINT,
  characterId VARCHAR(255),
  FOREIGN KEY (characterId) REFERENCES characters(id)
) DEFAULT CHARSET utf8 COMMENT '';