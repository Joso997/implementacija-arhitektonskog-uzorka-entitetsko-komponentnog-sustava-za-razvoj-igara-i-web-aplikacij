<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class RegionController extends Controller
{
    const Title = 0;
    const Value = 1;
    const Design = 2;
    const Label = 3;
    const Row = 0;
    const Field = 1;
    const Button = 2;
    const Text = 3;
    const ShowResolve = 4;
    const ContentToolkitComponent = 5;
    public function resolveRegion(){
        return response(
            [
                [
                    "Stats" =>
                    [
                        self::Title =>["Data" => "Naslov"],
                        self::Value => ["Data" => null],
                        self::Design =>["Data" => "form-control"],
                        self::Label =>["Data" => "Title"]
                    ],
                    "Region" => 0,
                    "ObjectEnum" => self::Field,
                    "SubObjectEnum" => 0,
                    "ActionEnum" => 2
                ],
                [
                    "Stats" =>
                        [
                            self::Title =>[
                                "Data" => "Url Slike"
                            ],
                            self::Value => [
                                "Data" => null
                            ],
                            self::Design =>[
                                "Data" => "form-control"
                            ],
                            self::Label =>[
                                "Data" => "Image"
                            ]
                        ],
                    "Region" => 0,
                    "ObjectEnum" => self::Field,
                    "SubObjectEnum" => 0,
                    "ActionEnum" => 3
                ],
                [
                    "Stats" =>
                        [
                            self::Title =>[
                                "Data" => "Autor"
                            ],
                            self::Value => [
                                "Data" => null
                            ],
                            self::Design =>[
                                "Data" => "form-control"
                            ],
                            self::Label =>[
                                "Data" => "Author"
                            ]
                        ],
                    "Region" => 0,
                    "ObjectEnum" => self::Field,
                    "SubObjectEnum" => 0,
                    "ActionEnum" => 2
                ],
                [
                    "Stats" =>
                        [
                            self::Title =>[
                                "Data" => "Submit"
                            ],
                            self::Value =>[
                                "Data" => null
                            ],
                            self::Design =>[
                                "Data" => "btn btn-primary"
                            ],
                            self::Label =>[
                                "Data" => "ContentToolkit"
                            ]
                        ],
                    "Region" => 0,
                    "ObjectEnum" => self::ContentToolkitComponent,
                    "SubObjectEnum" => 0,
                    "ActionEnum" => 0
                ],
                [
                    "Stats" =>
                        [
                            self::Title =>[
                                "Data" => "Submit"
                            ],
                            self::Value =>[
                                "Data" => null
                            ],
                            self::Design =>[
                                "Data" => "btn btn-primary"
                            ],
                            self::Label =>[
                                "Data" => "SubmitButton"
                            ]
                        ],
                    "Region" => 0,
                    "ObjectEnum" => self::Button,
                    "SubObjectEnum" => 1,
                    "ActionEnum" => 1
                ]

            ]
        );
    }
}
