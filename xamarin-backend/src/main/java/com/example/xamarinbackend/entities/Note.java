package com.example.xamarinbackend.entities;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.CascadeType;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Inheritance;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import java.util.List;
import java.util.UUID;

@Entity
@Getter
@Setter
public class Note extends IdEntity {

    private UUID accountId;
    private String title;
    private String color;
    private Double latitude;
    private Double longitude;

    @OneToMany(fetch = FetchType.EAGER, cascade = CascadeType.ALL, orphanRemoval = true)
    private List<TextElement> textElements = new java.util.ArrayList<>();


/*    @ManyToOne
    @JoinColumn(name = "account_id")
    private Account account;*/

}
