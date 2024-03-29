CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture',
  coverImg VARCHAR(255) DEFAULT 'https://cdn.pixabay.com/photo/2023/06/26/15/21/ai-generated-8090013_1280.png' COMMENT 'User Cover Image'
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE accounts;

CREATE TABLE characters(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  picture JSON NOT NULL,
  hp TINYINT NOT NULL,
  maxHp TINYINT NOT NULL,
  tempHp TINYINT DEFAULT 0,
  armorClass TINYINT DEFAULT 0,
  speed TINYINT NOT NULL,
  hitDie TINYINT NOT NULL,
  LEVEL TINYINT DEFAULT 1,
  xp MEDIUMINT DEFAULT 0,
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
  bonus JSON NOT NULL,
  skills JSON,
  proficiencies JSON,
  charFeatures JSON,
  cantrips JSON,
  spells JSON,
  casting JSON,
  equipment JSON,
  currency JSON NOT NULL,
  armor JSON,
  weapons JSON,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE characters;

SELECT
  *
FROM
  information_schema.TABLES
WHERE
  TABLE_NAME = 'characters';

ALTER TABLE
  characters
ADD
  COLUMN currency JSON NOT NULL;

CREATE TABLE campaigns(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  description VARCHAR(1000),
  privateNotes JSON,
  publicNotes JSON,
  EVENTS JSON,
  monsters JSON,
  initiative JSON,
  creatorId VARCHAR(255) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE campaigns;

CREATE TABLE npcs(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  picture VARCHAR(255) NOT NULL,
  class VARCHAR(20) NOT NULL,
  race VARCHAR(20) NOT NULL,
  characterId VARCHAR(128) NOT NULL,
  campaignId VARCHAR(128) NOT NULL,
  FOREIGN KEY (characterId) REFERENCES characters(id) ON DELETE CASCADE,
  FOREIGN KEY (campaignId) REFERENCES campaigns(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE npcs;

CREATE TABLE players(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name VARCHAR(100) NOT NULL,
  picture VARCHAR(255) NOT NULL,
  class VARCHAR(20) NOT NULL,
  race VARCHAR(20) NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  characterId VARCHAR(128) NOT NULL,
  campaignId VARCHAR(128) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE,
  FOREIGN KEY (characterId) REFERENCES characters(id) ON DELETE CASCADE,
  FOREIGN KEY (campaignId) REFERENCES campaigns(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';

DROP TABLE players;

CREATE TABLE comments(
  id VARCHAR(128) NOT NULL PRIMARY KEY COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  description VARCHAR(100) NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  campaignId VARCHAR(128) NOT NULL,
  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE,
  FOREIGN KEY (campaignId) REFERENCES campaigns(id) ON DELETE CASCADE
) DEFAULT CHARSET utf8 COMMENT '';