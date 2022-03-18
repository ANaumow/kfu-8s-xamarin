package com.example.xamarinbackend.entities;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Entity;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

@Entity
@Getter
@Setter
public class TextElement extends IdEntity {

    public String noteId;
    public long x;
    public long y;
    public String text;
    public int fontSize;

}
