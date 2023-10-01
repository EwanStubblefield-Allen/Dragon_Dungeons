CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture',
  coverImg VARCHAR(255) COMMENT 'User Cover Image'
) DEFAULT CHARSET utf8 COMMENT '';

CREATE TABLE characters(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  picture VARCHAR(1000) NOT NULL,
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
  age SMALLINT NOT NULL,
  feet TINYINT NOT NULL,
  inches TINYINT NOT NULL,
  weight SMALLINT NOT NULL,
  eyes VARCHAR(30) NOT NULL,
  skin VARCHAR(30) NOT NULL,
  hair VARCHAR(30) NOT NULL,
  features VARCHAR(1000) NOT NULL,
  background VARCHAR(100) NOT NULL,
  backstory VARCHAR(1000) NOT NULL,
  personalityTraits VARCHAR(500) NOT NULL,
  ideals VARCHAR(500) NOT NULL,
  bonds VARCHAR(500) NOT NULL,
  flaws VARCHAR(500) NOT NULL,
  manual BOOLEAN DEFAULT false,
  str TINYINT NOT NULL,
  dex TINYINT NOT NULL,
  con TINYINT NOT NULL,
  intelligence TINYINT NOT NULL,
  wis TINYINT NOT NULL,
  cha TINYINT NOT NULL,
  skills JSON,
  proficiencies JSON,
  cantrips JSON,
  spells JSON,
  equipment JSON,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE characters;

CREATE TABLE bonuses(
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
  FOREIGN KEY (characterId) REFERENCES characters(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE bonuses;